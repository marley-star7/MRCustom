
namespace MRCustom;

/// <summary>
/// Used to represent an explosion for "IReactToExplosion" interfaces while not actually using explosion.
/// </summary>
public class ProxyExplosion : Explosion
{
    public ProxyExplosion(Room room, PhysicalObject sourceObject, Vector2 pos, int lifeTime, float rad, float force, float damage, float stun, float deafen, Creature killTagHolder, float killTagHolderDmgFactor, float minStun, float backgroundNoise) 
        : base(room, sourceObject, pos, lifeTime, rad, force, damage, stun, deafen, killTagHolder, killTagHolderDmgFactor, minStun, backgroundNoise)
    {
    }

    public override void Update(bool eu)
    {
        evenUpdate = eu;
        Destroy();
    }
}
