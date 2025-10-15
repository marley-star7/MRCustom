/*
using MRCustom.Modules.BodyChunks;
using MRCustom.Extensions.Creatures;

namespace MRCustom.Modules.PhysicalObjects.PlayerCarryableItems;

public class BodyChunkProtectionItemModule : PlayerCarryableItemModule
{
    public BodyChunkProtection bodyChunkProtection;

    public int protectingBodyChunkIndex = 0;

    public BodyChunkProtectionItemModule()
    {
        bodyChunkProtection = new BodyChunkProtection();
    }

    //-- MS7: Have to run the code in here for blocking spear hits thanks to downpour and base game code.
    public virtual bool PreSpearHitWearer(Spear spear, SharedPhysics.CollisionResult result, bool eu)
    {
        if (result.chunk.index != protectingBodyChunkIndex)
            return true; // The spear acts as normal if not hit the armored chunk.

        // Do violence to player to stun them.
        if (owner.TryGetPhysicalObjectModule<HealthModule>(out var healthModule))
        {
            healthModule.TakeDamage(durabilityDamage);
        }

        bodyChunkProtection.PreSpearHitWearer(spear, result, eu);

        return false;
    }

    public void Destroy(Vector2 pos)
    {
        if (owner.TryGetPhysicalObjectModule<LizardShellEffectsModule>(out var lizardShellEffectsModule))
        {
            lizardShellEffectsModule.DoShatterEffects(owner.firstChunk.pos);
        }

        owner.Destroy();
    }
}
*/