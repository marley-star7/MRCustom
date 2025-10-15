/*
using MRCustom.Modules.BodyChunks;
using MRCustom.Extensions;
using MRCustom.Extensions.Creatures;

namespace MRCustom.Modules.PhysicalObjects.Creatures;

public class DamageProtectionCreatureModule : CreatureModule
{
    public int protectingBodyChunkIndex = 0;

    public DamageProtectionCreatureModule(Creature owner) : base(owner)
    {
    }

    public void Destroy(Vector2 pos)
    {
        if (owner.TryGetPhysicalObjectModule<LizardShellEffectsModule>(out var lizardShellEffectsModule))
        {
            lizardShellEffectsModule.DoShatterEffects(owner.firstChunk.pos);
        }

        owner.Destroy();
    }

    //-- MS7: Have to run the code in here for blocking spear hits thanks to downpour and base game code.
    public virtual bool PreSpearHitWearer(Spear spear, SharedPhysics.CollisionResult result, bool eu)
    {
        if (result.chunk.index != protectingBodyChunkIndex)
            return true; // The spear acts as normal if not hit the armored chunk.

        // Spear bounces off.
        spear.vibrate = 20;
        spear.ChangeMode(Weapon.Mode.Free);
        spear.firstChunk.vel = spear.firstChunk.vel * -0.5f + Custom.DegToVec(Random.value * 360f) * Mathf.Lerp(0.1f, 0.4f, Random.value) * spear.firstChunk.vel.magnitude;
        spear.SetRandomSpin();

        // Do violence to player to stun them.
        if (owner.TryGetPhysicalObjectModule<HealthModule>(out var healthModule))
        {
            healthModule.TakeDamage(durabilityDamage);
        }

        var damage = 0f;
        var stunBonus = MarCreatureExtensions.ConvertDamageToStunBonus(durabilityDamage);
        var directionAndMomentum = -spear.firstChunk.vel;

        owner.Violence(spear.firstChunk, directionAndMomentum, result.chunk, null, Creature.DamageType.Blunt, damage, stunBonus);

        return false;
    }

    public virtual void PreWearerViolence(ViolenceContext violenceContext)
    {
        if (violenceContext.hitChunk == null || violenceContext.hitChunk.index != protectingBodyChunkIndex)
            return; // This accessory does not modify player.

        //
        // Deflection of damage.
        //

        if (violenceContext.type == Creature.DamageType.Bite || violenceContext.type == Creature.DamageType.Stab)
        {
            violenceContext.type = Creature.DamageType.Blunt;
        }

        var durabilityDamage = violenceContext.damage / AccessoryProperties.Toughness;
        TakeRawDurabilityDamage(durabilityDamage);

        violenceContext.damage = 0f; // As long as there is any bit of helmet left on you, no damage is dealt.
        violenceContext.stunBonus += MarCreatureExtensions.ConvertDamageToStunBonus(durabilityDamage);

        Vector2 directionAndMomentum;
        if (violenceContext.directionAndMomentum == null)
        {
            //-- MS7: Have to check source, necessary bug fix for explosive spears which can destroy themselves.
            if (violenceContext.source == null)
                return; // No source, no direction and momentum.

            //-- MS7: If the direction and momentum is not set, then we use the source position and hit chunk position to calculate some roughly close value.
            directionAndMomentum = violenceContext.source.pos - violenceContext.hitChunk.pos;
        }
        else
            directionAndMomentum = violenceContext.directionAndMomentum.Value;

        if (owner.TryGetPhysicalObjectModule)
            lizardShellArmor.DoDeflectEffects(wearer.bodyChunks[protectingBodyChunkIndex], violenceContext.hitChunk.pos, directionAndMomentum, violenceContext.damage, violenceContext.stunBonus);
    }
}
*/