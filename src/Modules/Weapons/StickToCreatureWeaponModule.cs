using MRCustom.Modules.PlayerCarryableItems;
using static PhysicalObject;

namespace MRCustom.Modules.Weapons;

public class StickToCreatureWeaponModule : PlayerCarryableItemModule, IModuleUsingAbstractObjectSticks
{
    public new Weapon Owner => (Weapon)_owner;
    /// <summary>
    /// The chunk of the owner that is used in sticking logic.
    /// </summary>
    public BodyChunk ownerChunkToStick;

    private PhysicalObject? _stuckToObject = null;
    /// <summary>
    /// The Physical Object this Weapon is stuck in.
    /// </summary>
    public PhysicalObject? StuckToObject => _stuckToObject;

    private int _stuckToChunkIndex = -1;
    /// <summary>
    /// The chunk index of the stuck in object's body chunks this weapon is stuck in.
    /// </summary>
    public int StuckToChunkIndex => _stuckToChunkIndex;

    /// <summary>
    /// The chunk this weapon is stuck in.
    /// </summary>
    public BodyChunk? StuckToChunk => StuckToObject?.bodyChunks[StuckToChunkIndex];

    private Appendage.Pos? _stuckToAppendage;
    /// <summary>
    /// The appendage this weapon is stuck in.
    /// </summary>
    public Appendage.Pos? StuckToAppendage => _stuckToAppendage;

    /// <summary>
    /// The rotation set when sticking.
    /// </summary>
    public virtual Vector2 RotationSetOnStick => Owner.rotation;

    public SoundID? stickSound = null;
    public SoundID? unstickSound = null;

    public float stuckRotation;

    public StickToCreatureWeaponModule(Weapon owner, BodyChunk ownerChunkToStick) : base(owner, typeof(StickToCreatureWeaponModule))
    {
        this.ownerChunkToStick = ownerChunkToStick;
    }

    public void Update(bool eu)
    {
        if (Owner.mode == Weapon.Mode.StuckInCreature)
        {
            if (_stuckToAppendage != null)
            {
                Owner.setRotation = Custom.DegToVec(stuckRotation + Custom.VecToDeg(_stuckToAppendage.appendage.OnAppendageDirection(_stuckToAppendage)));
                ownerChunkToStick.pos = _stuckToAppendage.appendage.OnAppendagePosition(_stuckToAppendage);
            }
            else
            {
                ownerChunkToStick.vel = StuckToChunk.vel;
                Owner.setRotation = Custom.DegToVec(stuckRotation + Custom.VecToDeg(StuckToChunk.Rotation));
                ownerChunkToStick.MoveWithOtherObject(eu, StuckToChunk, new Vector2(0f, 0f));
            }
            if (StuckToChunk.owner.slatedForDeletetion)
            {
                Owner.ChangeMode(Weapon.Mode.Free);
            }
        }
    }

    public void RecreateSticksFromAbstract()
    {
        for (int i = 0; i < Owner.abstractPhysicalObject.stuckObjects.Count; i++)
        {
            if (Owner.abstractPhysicalObject.stuckObjects[i] is AbstractPhysicalObject.AbstractSpearStick 
                && (Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearStick).Spear == Owner.abstractPhysicalObject 
                && (Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearStick).LodgedIn.realizedObject != null)
            {
                AbstractPhysicalObject.AbstractSpearStick abstractSpearStick = Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearStick;
                _stuckToObject = abstractSpearStick.LodgedIn.realizedObject;
                _stuckToChunkIndex = abstractSpearStick.chunk;

                stuckRotation = abstractSpearStick.angle;
                Owner.ChangeMode(Weapon.Mode.StuckInCreature);
            }
            else if (Owner.abstractPhysicalObject.stuckObjects[i] is AbstractPhysicalObject.AbstractSpearAppendageStick 
                && (Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearAppendageStick).Spear == Owner.abstractPhysicalObject 
                && (Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearAppendageStick).LodgedIn.realizedObject != null)
            {
                AbstractPhysicalObject.AbstractSpearAppendageStick abstractSpearAppendageStick = Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearAppendageStick;
                _stuckToObject = abstractSpearAppendageStick.LodgedIn.realizedObject;
                _stuckToAppendage = new PhysicalObject.Appendage.Pos(_stuckToObject.appendages[abstractSpearAppendageStick.appendage], abstractSpearAppendageStick.prevSeg, abstractSpearAppendageStick.distanceToNext);

                stuckRotation = abstractSpearAppendageStick.angle;
                Owner.ChangeMode(Weapon.Mode.StuckInCreature);
            }
        }
    }

    public virtual void StickToChunk(BodyChunk onChunk, bool eu)
    {
        PlayStickSound();

        new AbstractPhysicalObject.AbstractSpearStick(
            Owner.abstractPhysicalObject, 
            onChunk.owner.abstractPhysicalObject, 
            onChunk.index, 
            -1, 
            this.stuckRotation
        );

        _stuckToObject = onChunk.owner;
        _stuckToChunkIndex = onChunk.index;
        _stuckToAppendage = null;

        stuckRotation = Custom.VecToDeg(RotationSetOnStick);
        Owner.rotationSpeed = 0;

        ownerChunkToStick.MoveWithOtherObject(eu, StuckToChunk, new Vector2(0f, 0f));

        Owner.ChangeMode(Weapon.Mode.StuckInCreature);
    }

    public virtual void StickToAppendage(Appendage.Pos onAppendagePos)
    {
        PlayStickSound();

        new AbstractPhysicalObject.AbstractSpearAppendageStick(
            Owner.abstractPhysicalObject, 
            onAppendagePos.appendage.owner.abstractPhysicalObject, 
            onAppendagePos.appendage.appIndex, 
            onAppendagePos.prevSegment, 
            onAppendagePos.distanceToNext, 
            this.stuckRotation
        );

        _stuckToObject = onAppendagePos.appendage.owner;
        _stuckToChunkIndex = 0;
        _stuckToAppendage = onAppendagePos;

        stuckRotation = Custom.VecToDeg(RotationSetOnStick) - Custom.VecToDeg(_stuckToAppendage.appendage.OnAppendageDirection(_stuckToAppendage));
        Owner.rotationSpeed = 0;

        Owner.ChangeMode(Weapon.Mode.StuckInCreature);
    }

    public virtual void StickToCollisionResult(SharedPhysics.CollisionResult result, bool eu)
    {
        if (result.chunk != null)
        {
            StickToChunk(result.chunk, eu);
        }
        else if (result.onAppendagePos != null)
        {
            StickToAppendage(result.onAppendagePos);
        }

        // Makes it overlap ontop of the thing properly.
        Owner.ChangeOverlap(true);
        Owner.ChangeOverlap(false);
    }

    public virtual void PullFromStuckObject()
    {
        Owner.rotationSpeed = 0;
        if (Owner.room != null)
        {
            PlayUnstickSound();
        }

        for (int i = 0; i < Owner.abstractPhysicalObject.stuckObjects.Count; i++)
        {
            if (Owner.abstractPhysicalObject.stuckObjects[i] is AbstractPhysicalObject.AbstractSpearStick && (Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearStick).Spear == Owner.abstractPhysicalObject)
            {
                Owner.abstractPhysicalObject.stuckObjects[i].Deactivate();
                break;
            }
            if (Owner.abstractPhysicalObject.stuckObjects[i] is AbstractPhysicalObject.AbstractSpearAppendageStick && (Owner.abstractPhysicalObject.stuckObjects[i] as AbstractPhysicalObject.AbstractSpearAppendageStick).Spear == Owner.abstractPhysicalObject)
            {
                Owner.abstractPhysicalObject.stuckObjects[i].Deactivate();
                break;
            }
        }
        _stuckToObject = null;
        _stuckToAppendage = null;
        _stuckToChunkIndex = 0;
    }

    public virtual void PlayStickSound()
    {
        if (stickSound != null)
        {
            Owner.room.PlaySound(stickSound, ownerChunkToStick, false, 1f, Random.Range(1.4f, 1.8f));
        }
    }

    public virtual void PlayUnstickSound()
    {
        if (unstickSound != null)
        {
            Owner.room.PlaySound(unstickSound, ownerChunkToStick, false, 1f, Random.Range(1.4f, 1.8f));
        }
    }
}
