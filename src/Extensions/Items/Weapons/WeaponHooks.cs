using static System.ActivationContext;

namespace MRCustom.Extensions.Items.Weapons;

public static class WeaponHooks
{
    internal static void ApplyHooks()
    {
        On.Weapon.Thrown += Weapon_Thrown;
    }

    internal static void RemoveHooks()
    {
        On.Weapon.Thrown += Weapon_Thrown;

    }

    private static void Weapon_Thrown(On.Weapon.orig_Thrown orig, Weapon self, Creature thrownBy, Vector2 thrownPos, Vector2? firstFrameTraceFromPos, IntVector2 throwDir, float frc, bool eu)
    {
        WeaponThrownContext context = new(thrownBy, thrownPos, firstFrameTraceFromPos, throwDir, frc);

        if (thrownBy.TryGetModule<WeaponThrowContextModifierCreatureModule>(out var weaponThrowModifierModule))
        {
            weaponThrowModifierModule.OnWeaponThrown(self, context, eu);

            orig(self, context.thrownBy, context.thrownPos, context.firstFrameTraceFromPos, context.throwDir, context.force, eu);

            weaponThrowModifierModule.PostWeaponThrown(self);
        }
        else
        {
            orig(self, context.thrownBy, context.thrownPos, context.firstFrameTraceFromPos, context.throwDir, context.force, eu);
        }
    }
}
