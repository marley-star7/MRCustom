/*
namespace MRCustom.Physics;

public class VultureMaskRotationStrategy : RotationStrategy
{
    public IRotatable owner;

    public Vector2 rotVel;

    public override void Update()
    {
        owner.rotation = Custom.DegToVec(Custom.VecToDeg(owner.rotation) + rotVel.x);
        rotVel = Vector2.ClampMagnitude(rotVel, 50f);
        rotVel *= Custom.LerpMap(rotVel.magnitude, 5f, 50f, 1f, 0.8f);

        HandleGroundCollision();
    }

    public override void DrawSpritesUpdate(float timeStacker)
    {
        rotationTimeStacked = Vector3.Slerp(owner.lastRotation, owner.rotation, timeStacker);
    }

    private void HandleGroundCollision()
    {
        if (owner.firstChunk.ContactPoint.y < 0)
        {
            Vector2 groundRotation = Custom.DegToVec(90f * (facingRight ? 1 : -1));
            owner.rotation = Vector2.Lerp(owner.rotation, groundRotation, UnityEngine.Random.value);
            rotVel *= UnityEngine.Random.value;
        }
        else if (Vector2.Distance(owner.firstChunk.lastPos, owner.firstChunk.pos) > 5f && rotVel.magnitude < 7f)
        {
            rotVel += Custom.RNV() * (Mathf.Lerp(7f, 25f, UnityEngine.Random.value) + owner.firstChunk.vel.magnitude * 2f);
        }
    }
}
*/
