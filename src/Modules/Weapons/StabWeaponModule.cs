namespace MRCustom.Modules.Weapons;

public class StabWeaponModule : PlayerCarryableItemModule, IOverridePlayerPressingGrabInput
{
    protected OverridePlayerPressingGrabInputItemModule _overridePlayerPressingGrabInputModule;
    public OverridePlayerPressingGrabInputItemModule OverridePlayerPressingGrabInputModule => _overridePlayerPressingGrabInputModule;

    public SoundID stabSound = SoundID.Spear_Stick_In_Creature;

    public int stabTimer;

    public int timeRequiredToInitaiteStab = 15;
    public int timeStabOccurs = 30;

    public float bloodVelocityMultiplier = 12f;

    public PlayerHandAnimationPlayer.AnimationIndex playerHandAnimation = Enums.PlayerHandAnimations.KnifeStab;

    /// <summary>
    /// How much damage the stab deals.
    /// </summary>
    public float damage = 1f;
    public int stunTime = 5;

    public StabWeaponModule(Weapon Owner) : base(Owner, typeof(StabWeaponModule))
    {
        this._overridePlayerPressingGrabInputModule = new OverridePlayerPressingGrabInputItemModule(Owner, this);
        Owner.AddModule(_overridePlayerPressingGrabInputModule);
    }

    private Vector2 GetWeirdAssVector(in BodyChunk creatureGraspChunk)
    {
        Vector2 vector = creatureGraspChunk.pos * creatureGraspChunk.mass;
        float num = creatureGraspChunk.mass;
        for (int i = 0; i < creatureGraspChunk.owner.bodyChunkConnections.Length; i++)
        {
            if (creatureGraspChunk.owner.bodyChunkConnections[i].chunk1 == creatureGraspChunk)
            {
                vector += creatureGraspChunk.owner.bodyChunkConnections[i].chunk2.pos * creatureGraspChunk.owner.bodyChunkConnections[i].chunk2.mass;
                num += creatureGraspChunk.owner.bodyChunkConnections[i].chunk2.mass;
            }
            else if (creatureGraspChunk.owner.bodyChunkConnections[i].chunk2 == creatureGraspChunk)
            {
                vector += creatureGraspChunk.owner.bodyChunkConnections[i].chunk1.pos * creatureGraspChunk.owner.bodyChunkConnections[i].chunk1.mass;
                num += creatureGraspChunk.owner.bodyChunkConnections[i].chunk1.mass;
            }
        }
        vector /= num;

        return vector;
    }

    protected virtual void SplurtBlood(BodyChunk fromChunk, Vector2 direction, float directionDegreesVariance = 90f)
    {
        var directionDegrees = Custom.VecToDeg(direction);

        for (int j = UnityEngine.Random.Range(8, 14); j >= 0; j--)
        {
            Owner.room.AddObject(new WaterDrip(
                pos: fromChunk.pos + (fromChunk.rad * Custom.RNV() * UnityEngine.Random.value),
                vel: (
                    // Add velocity of the chunk it's stabbed from for more realism blood!!!
                    fromChunk.vel + Custom.DegToVec(
                    // A random direction in the cone of variance from initial direction.
                        Mathf.Lerp(
                            directionDegrees - directionDegreesVariance, 
                            directionDegrees + directionDegreesVariance, 
                            UnityEngine.Random.value) / 2f
                        ) 
                    * bloodVelocityMultiplier * UnityEngine.Random.value // Add magnitude with bloodVelocity.
                ) * Owner.EffectiveRoomGravity, // If room gravity is low blood just floats there instead.
                waterColor: false
            ));
        }
    }

    public void Stab(Creature creatureStabbed, Creature.Grasp creatureGrasp, BodyChunk chunkStabbed)
    {
        DoStabEffects(chunkStabbed);
        DoStabViolence(creatureStabbed, chunkStabbed);

        ResetStab();
    }

    public virtual void DoStabViolence(Creature creatureStabbed, BodyChunk chunkStabbed)
    {
        creatureStabbed.SetKillTag(Owner.grabbedBy[0].grabber.abstractCreature);
        creatureStabbed.Violence(Owner.firstChunk, new Vector2?(new Vector2(0f, 0f)), chunkStabbed, null, Creature.DamageType.Stab, damage, 15f);
        creatureStabbed.stun = stunTime;

        if (!creatureStabbed.dead)
        {
            if (creatureStabbed.abstractCreature.creatureTemplate.type == DLCSharedEnums.CreatureTemplateType.Inspector)
            {
                creatureStabbed.Die();
            }

            //-- Ms7: Disabled for quality of life, you are just going to re-grab them anyway lol.

            // Don't let go of the creature if it died that stab.
            //creatureGrasp.Release();
        }
    }

    public virtual void DoStabEffects(BodyChunk fromChunk)
    {
        fromChunk.owner.room.PlaySound(stabSound, fromChunk);

        SplurtBlood(fromChunk, Custom.DirVec(Owner.firstChunk.pos, fromChunk.pos));
    }

    // Ms7: Uhhhh, ignore this.
    private void DoStabEffectsOld(Player player, BodyChunk fromChunk)
    {
        player.mainBodyChunk.pos += Custom.DegToVec(Mathf.Lerp(-90f, 90f, UnityEngine.Random.value)) * 4f;
        fromChunk.vel += Custom.DirVec(GetWeirdAssVector(fromChunk), player.mainBodyChunk.pos) * 0.9f / fromChunk.mass;

        SplurtBlood(fromChunk, Custom.DirVec(fromChunk.pos, Owner.firstChunk.pos));
    }

    public void PlayerJustPressedGrabUpdate(Player player)
    {

    }

    public void PlayerPressingGrabUpdate(Player player)
    {
        var otherHandGraspIndex = MarPlayerExtensions.GetOtherGrasp(Owner.grabbedBy[0].graspUsed);

        if (Owner.grabbedBy[0] != null && player.grasps[otherHandGraspIndex] != null)
        {
            // Stab update if other hand has something to stab.
            StabUpdate(player.grasps[otherHandGraspIndex]);
        }
    }

    public void PlayerJustReleasedGrabUpdate(Player player)
    {
        player.GetHandAnimationPlayer().Stop(playerHandAnimation);
        ResetStab();
    }

    public void PlayerNotPressingGrabUpdate(Player player)
    {
    }

    public void ResetStab()
    {
        this.stabTimer = 0;
    }

    protected void PlayPlayerAnimation(Player player)
    {
        // Set the primary hand for the animation to be duh thing.
        player.GetMarPlayerData().AnimationPrimaryHandIndex = Owner.grabbedBy[0].graspUsed;
        player.GetHandAnimationPlayer().Play(playerHandAnimation);
    }

    public virtual void ActualStabUpdate(Creature.Grasp creatureGrasp)
    {
        var player = creatureGrasp.grabber as Player;
        PlayPlayerAnimation(player);

        player.Blink(5);

        if (this.stabTimer % timeRequiredToInitaiteStab == 0)
        {
            Vector2 b = Custom.RNV() * 3f;
            player.mainBodyChunk.pos += b;
            player.mainBodyChunk.vel += b;
        }

        if (this.stabTimer % timeStabOccurs == 0)
        {
            Stab(creatureGrasp.grabbed as Creature, creatureGrasp, creatureGrasp.grabbedChunk);
        }
    }

    public void StabUpdate(Creature.Grasp creatureGrasp)
    {
        if (creatureGrasp == null || !(creatureGrasp.grabbed is Creature creatureGrabbed) || creatureGrasp.grabber is not Player player)
        {
            return;
        }

        // Don't stab dead creatures.
        if (creatureGrabbed.dead)
        {
            ResetStab();
            player.wantToPickUp = 0;

            return;
        }

        if (creatureGrabbed.Consious && !this.IsCreatureLegalToHoldWithoutStun(creatureGrabbed))
        {
            ResetStab();
            player.wantToPickUp = 0;
            creatureGrasp.Release();

            Plugin.LogDebug("Lost Hold of Stab Target");

            return;
        }

        // Don't pull the spear off the back if we are STABBIN.
        if (player.spearOnBack != null)
        {
            player.spearOnBack.increment = false;
            player.spearOnBack.interactionLocked = true;
        }
        if (player.slugOnBack != null)
        {
            player.slugOnBack.increment = false;
            player.slugOnBack.interactionLocked = true;
        }

        // Increment timer.
        stabTimer++;

        // Don't eat when stabbing please.
        player.eatCounter = Consts.Player.EatCounterResetValue;
        player.eatMeat = 0;
        // Don't maul either when stabbing, individual stab functionality should override maul's
        player.maulTimer = 0;

        if (this.stabTimer > timeRequiredToInitaiteStab)
        {
            ActualStabUpdate(creatureGrasp);
        }

        /*
        if (player.graphicsModule != null)
        {
            if (this._stabTimer > 10 && this._stabTimer % 8 == 3)
            {
                DoStabEffects(player, creatureGrasp.grabbedChunk);
                return;
            }
        }
        */
    }

    public virtual bool IsCreatureLegalToHoldWithoutStun(Creature grabCheck)
    {
        return grabCheck is JetFish
            || grabCheck is LanternMouse
            || grabCheck is Fly
            || grabCheck is TubeWorm
            || grabCheck is Snail
            || grabCheck is EggBug
            || grabCheck is Player
            || grabCheck is Cicada
            || (grabCheck is Centipede && (grabCheck as Centipede).Small)
            || grabCheck is SmallNeedleWorm
            || (grabCheck is Barnacle && !(grabCheck as Barnacle).hasShell)
            || grabCheck is Yeek
            // All additions below this point are different from basegame's Player.IsCreatureLegalToHoldWithoutStun.
            || grabCheck is VultureGrub; // Vulture grub added because your literally holding it lol
    }
}
