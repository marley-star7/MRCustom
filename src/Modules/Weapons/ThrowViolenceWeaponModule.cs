using static PhysicalObject;

namespace MRCustom.Modules.Weapons;

public class ThrowViolenceWeaponModule : PlayerCarryableItemModule
{
    public new Weapon owner;

    /// <summary>
    /// Extra multiplication on the normal knockback.
    /// </summary>
    public float creatureKnockbackMultiplier = 2f;
    /// <summary>
    /// The damage dealt on throw weapon hit.
    /// </summary>
    public float damageBonus = 1f;
    /// <summary>
    /// The bonus stun time dealt on throw hit creature.
    /// </summary>
    public float stunBonus = 10f;

    /// <summary>
    /// The sound made when hit
    /// </summary>
    public SoundID? hitSound = null;

    /// <param name="owner"></param>
    public ThrowViolenceWeaponModule(Weapon owner) : base(owner, typeof(ThrowViolenceWeaponModule))
    {
        this.owner = owner;
    }

    public virtual void PlayHitSound(BodyChunk source)
    {
        if (hitSound == null)
        {
            owner.room.PlaySound(hitSound, source, false, 1f, Random.Range(1.4f, 1.8f));
        }
    }

    public virtual void HitCreature(BodyChunk source, Creature hitCreature, Vector2 directionAndMomentum, BodyChunk hitChunk, Appendage.Pos hitAppendage)
    {
        hitCreature.Violence(source, directionAndMomentum, hitChunk, hitAppendage, Creature.DamageType.Stab, damageBonus, stunBonus);
        PlayHitSound(source);
    }

    /// <param name="source"></param>
    /// <param name="result"></param>
    /// <param name="eu"></param>
    /// <returns></returns>
    public virtual bool HitSomething(BodyChunk source, SharedPhysics.CollisionResult result, bool eu)
    {
        if (result.obj == null)
            return false;
        if (result.obj.abstractPhysicalObject.rippleLayer != owner.abstractPhysicalObject.rippleLayer && !result.obj.abstractPhysicalObject.rippleBothSides && !owner.abstractPhysicalObject.rippleBothSides)
            return false;

        if (result.obj is Creature)
        {
            var hitCreature = (Creature)result.obj;
            HitCreature(source, hitCreature, source.vel * source.mass * creatureKnockbackMultiplier, result.chunk, result.onAppendagePos);
            return true;
        }
        else if (result.chunk != null)
        {
            result.chunk.vel += source.vel * source.mass / result.chunk.mass; // TODO: for simplicity maybe add an extension method for ApplyForceOnChunk or something like that, so don't have to re-use formula constantly?
        }
        else if (result.onAppendagePos != null)
        {
            ((IHaveAppendages)result.obj).ApplyForceOnAppendage(result.onAppendagePos, source.vel * source.mass);
        }

        return false;
    }
}
