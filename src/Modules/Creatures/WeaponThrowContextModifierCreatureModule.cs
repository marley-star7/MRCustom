namespace MRCustom.Modules.Creatures;

public class WeaponThrowContextModifierCreatureModule : CreatureModule
{
    public float throwForce = 1;
    public float adrenalineModifier = 1.5f;

    public float? overrideExitThrowSpeed = null;

    public WeaponThrowContextModifierCreatureModule(Player owner) : base(owner, typeof(WeaponThrowContextModifierCreatureModule))
    {

    }

    public void OnWeaponThrown(Weapon weaponThrown, WeaponThrownContext weaponThrownContext, bool evenUpdate)
    {
        if (weaponThrownContext.thrownBy is Player player)
        {
            weaponThrownContext.force = Mathf.Lerp(throwForce, throwForce * adrenalineModifier, player.Adrenaline);
        }
        else
        {
            weaponThrownContext.force = throwForce;
        }
    }

    public void PostWeaponThrown(Weapon weaponThrown)
    {
        if (overrideExitThrowSpeed.HasValue)
        {
            weaponThrown.overrideExitThrownSpeed = overrideExitThrowSpeed.Value;
        }
    }
}
