using static Player;

namespace MRCustom.Extensions;

public class MarPlayerData
{
    public Player.AnimationIndex lastAnimation;

    public struct RunSpeedLinearModifier
    {
        public float value;
        public RunSpeedLinearModifier(float value)
        { 
            this.value = value; 
        }
    }

    private int _animationPrimaryHandIndex = 0;
    public int AnimationPrimaryHandIndex
    {
        get => _animationPrimaryHandIndex;
        set
        {
            _animationPrimaryHandIndex = value;
            _animationSecondaryHandIndex = MarPlayerExtensions.GetOtherGrasp(value);
        }
    }

    private int _animationSecondaryHandIndex = 1;
    public int AnimationSecondaryHandIndex
    {
        get => _animationSecondaryHandIndex;
        set
        {
            _animationSecondaryHandIndex = value;
            _animationPrimaryHandIndex = MarPlayerExtensions.GetOtherGrasp(value);
        }
    }

    /// <summary>
    /// Percentages that are added up before multiplied to the current base run speed value.
    /// </summary>
    public StatModifierByteId<RunSpeedLinearModifier> runSpeedLinearModifiers = new StatModifierByteId<RunSpeedLinearModifier>();

    public WeakReference<Player> playerRef;

    public MarPlayerData(Player player)
    {
        playerRef = new WeakReference<Player>(player);
    }
}

public static class MarPlayerExtensions
{
    private static readonly ConditionalWeakTable<Player, MarPlayerData> dataConditionalWeakTable = new();

    public static MarPlayerData GetMarPlayerData(this Player player) => dataConditionalWeakTable.GetValue(player, _ => new MarPlayerData(player));

    /// <summary>
    /// Switch the items in the player's hands.
    /// Stolen from source.
    /// </summary>
    /// <param name="player"></param>
    public static void SwitchHands(this Player player)
    {
        if (player.input[0].pckp && !player.input[1].pckp && player.switchHandsProcess == 0f && !player.isSlugpup)
        {
            bool flag4 = player.grasps[0] != null || player.grasps[1] != null;
            if (player.grasps[0] != null && (player.Grabability(player.grasps[0].grabbed) == ObjectGrabability.TwoHands || player.Grabability(player.grasps[0].grabbed) == ObjectGrabability.Drag))
            {
                flag4 = false;
            }
            if (flag4)
            {
                if (player.switchHandsCounter == 0)
                {
                    player.switchHandsCounter = 15;
                }
                else
                {
                    player.room.PlaySound(SoundID.Slugcat_Switch_Hands_Init, player.mainBodyChunk);
                    player.switchHandsProcess = 0.01f;
                    player.wantToPickUp = 0;
                    player.noPickUpOnRelease = 20;
                }
            }
            else
            {
                player.switchHandsProcess = 0f;
            }
        }
    }

    /// <summary>
    /// Realizes an abstractPhysicalObject into the room and grabs it.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="abstractPhysicalObject"></param>
    public static void RealizeAndGrab(this Player self, AbstractPhysicalObject abstractPhysicalObject)
    {
        if (abstractPhysicalObject.realizedObject == null)
            abstractPhysicalObject.RealizeInRoom();

        self.SlugcatGrab(abstractPhysicalObject.realizedObject, self.FreeHand());
    }

    /// <summary>
    /// Copied from source code for how the player ends a roll.
    /// </summary>
    /// <param name="self"></param>
    public static void EndRoll(this Player self)
    {
        self.rollCounter = 0;
        self.rollDirection = 0;
        self.room.PlaySound(SoundID.Slugcat_Roll_Finish, self.mainBodyChunk, loop: false, 1f, 1f);
        self.animation = AnimationIndex.None;
        self.standing = self.input[0].y > -1;
    }

    /// <summary>
    /// Gets the current threat level of the player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static float GetThreat(this Player player)
    {
        if (player == null)
            return 0f;
        if (player.abstractCreature.world.game.GameOverModeActive)
            return 0f;
        if (player.abstractCreature.world.game.manager.musicPlayer != null && player.abstractCreature.world.game.manager.musicPlayer.threatTracker != null)
            return player.abstractCreature.world.game.manager.musicPlayer.threatTracker.currentMusicAgnosticThreat;
        if (player.abstractCreature.world.game.manager.fallbackThreatDetermination == null)
            player.abstractCreature.world.game.manager.fallbackThreatDetermination = new ThreatDetermination(0);

        return player.abstractCreature.world.game.manager.fallbackThreatDetermination.currentMusicAgnosticThreat;
    }

    /// <summary>
    /// Returns the secondary hand grasp index if sent the primary, and primary hand index if sent the secondary
    /// Defaults to 0,
    /// </summary>
    /// <param name="graspIndex"></param>
    /// <returns></returns>
    public static int GetOtherGrasp(int graspIndex) =>
        graspIndex == 0 ? 1 : 0;
}
