namespace MRCustom.Modules.Weapons;

public class LodgeInCreatureWeaponModule : StickToCreatureWeaponModule
{
    public override Vector2 RotationSetOnStick => Owner.throwDir.ToVector2();

    public LodgeInCreatureWeaponModule(Weapon owner, BodyChunk ownerChunkToStick) : base(owner, ownerChunkToStick)
    {
        stickSound = SoundID.Spear_Stick_In_Creature;
    }

    public override void StickToCollisionResult(SharedPhysics.CollisionResult result, bool eu)
    {
        base.StickToCollisionResult(result, eu);

        if (Owner.room.BeingViewed)
        {
            for (int i = 0; i < 8; i++)
            {
				Owner.room.AddObject(new WaterDrip(result.collisionPoint, -ownerChunkToStick.vel * Random.value * 0.5f + Custom.DegToVec(360f * Random.value) * ownerChunkToStick.vel.magnitude * Random.value * 0.5f, waterColor: false));
            }
        }
    }
}
