using MRCustom.Modules.PhysicalObjects.Rotations;

namespace MRCustom.Modules.Weapons.Rotations;

public abstract class WeaponClassReferenceRotationModule : RotationModule
{
	public Weapon Owner => (Weapon)base.Owner;

	public override Vector2 Rotation
	{
		get => Owner.rotation;
		set => Owner.rotation = value;
	}
	public override Vector2 LastRotation
	{
		get => Owner.lastRotation;
		set => Owner.lastRotation = value;
	}

	public override Vector2? SetRotation
	{
		get => Owner.setRotation;
		set => Owner.setRotation = value;
	}

	public override void Update()
	{
		LogicUpdate();
	}


	public WeaponClassReferenceRotationModule(Weapon weapon) : base(weapon)
    {

    }
}