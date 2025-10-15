namespace MRCustom.Animations;

public class WeaponStabPlayerAnimation : RWAnimation<Player>
{
    public const int timeKnifeJutsToStab = 10;

    public override void Start(Player player)
    {

    }

    public override void Stop(Player player)
    {

    }

    public override void Update(Player player, float animationTimer)
    {

    }

    public override void GraphicsUpdate(Player player, float animationTimer)
    {
        var playerCraftingData = player.GetMarPlayerData();
        var playerGraphics = (PlayerGraphics)player.graphicsModule;

        var knifeHandIndex = GetKnifeHandIndex(player);
        var stabbingChunkHandIndex = GetStabbingChunkHandIndex(player);

        if (player.grasps[knifeHandIndex] == null || !player.grasps[knifeHandIndex].grabbed.TryGetModule<StabWeaponModule>(out var stabWeaponModule))
        {
            return;
        }

        var startTimeToJutKnifeTowardsTarget = stabWeaponModule.timeStabOccurs - timeKnifeJutsToStab;

        animationTimer = stabWeaponModule.stabTimer; // Use stab timer instead for more proper timing.

        Vector2 stabTargetPos;
        if (player.grasps[stabbingChunkHandIndex] != null)
        {
            stabTargetPos = player.grasps[stabbingChunkHandIndex].grabbedChunk.pos;
        }
        else // Fallback use other hands pos
        {
            stabTargetPos = playerGraphics.hands[stabbingChunkHandIndex].pos;
        }

        // Knife full out pos is the a path from the holding item to the body chunk.
        var dirStabTargetToBody = Custom.DirVec(stabTargetPos, player.bodyChunks[Consts.BodyChunkIndexes.Player.Head].pos);
        Vector2 knifeFullOutPos = player.bodyChunks[Consts.BodyChunkIndexes.Player.Head].pos + dirStabTargetToBody * 16; // The * 2 and normalization is so it starts further up.

        // Do the knife jut towards target for stab.
        if (animationTimer > startTimeToJutKnifeTowardsTarget)
        {
            //-- Ms7: Some math junk, just using a cos curve and cutting it off at full speed point to get the curve start.
            var jutKnifeProgress = Mathf.InverseLerp(
                -1, 
                0, 
                -Mathf.Cos(
                     ((animationTimer - startTimeToJutKnifeTowardsTarget) / timeKnifeJutsToStab) * (Mathf.PI / 2f)
                    )
                );
            jutKnifeProgress = Mathf.Pow(jutKnifeProgress, 4f); // Makes the speed up more exhaggurated.

            // TODO: use a bezier curve instead? make a function or something for it.
            playerGraphics.hands[knifeHandIndex].pos = Vector2.Lerp(knifeFullOutPos, stabTargetPos, jutKnifeProgress);
            playerGraphics.LookAtPoint(playerGraphics.hands[stabbingChunkHandIndex].pos, 69696);

            return;
        }
        else
        {
            // Slowly lerp towards position want to jut from.
            var moveToJutPosProgress = Mathf.InverseLerp(stabWeaponModule.timeRequiredToInitaiteStab, startTimeToJutKnifeTowardsTarget, animationTimer);
            playerGraphics.hands[knifeHandIndex].pos = Vector2.Lerp(playerGraphics.hands[knifeHandIndex].pos, knifeFullOutPos, moveToJutPosProgress);
        }
    }

    public int GetKnifeHandIndex(Player player)
    {
        return player.GetMarPlayerData().AnimationPrimaryHandIndex;
    }

    public int GetStabbingChunkHandIndex(Player player)
    {
        return player.GetMarPlayerData().AnimationSecondaryHandIndex;
    }
}