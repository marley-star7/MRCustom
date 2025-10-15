namespace MRCustom.Modules.PhysicalObjects.Rotations;

public class VelocityRotationModule : Vector3RotationModule
{
    public readonly BodyChunk mainBodyChunk;

    private Vector2 rotVel;

    public VelocityRotationModule(PhysicalObject owner, BodyChunk mainBodyChunk) : base(owner)
    {
        this.mainBodyChunk = mainBodyChunk;
    }

    protected override void LogicUpdate()
    {
        _rotation = Custom.DegToVec(Custom.VecToDeg(_rotation) + rotVel.x);
        rotVel = Vector2.ClampMagnitude(rotVel, 50f);
        rotVel *= Custom.LerpMap(rotVel.magnitude, 5f, 50f, 1f, 0.8f);

        if (mainBodyChunk.owner.grabbedBy.Count > 0
            && mainBodyChunk.owner.grabbedBy[0].grabber is Creature grabber)
        {
            _rotation = Custom.PerpendicularVector(Custom.DirVec(mainBodyChunk.pos, grabber.mainBodyChunk.pos));
            return; // Don't do ground collision handling if held.
        }

        if (mainBodyChunk.ContactPoint == IntVector2Extensions.zero)
        {
            // If we just left ground, set rotation velocity again.
            if (mainBodyChunk.lastContactPoint != IntVector2Extensions.zero)
            {
                SetRotationVelocity();
            }

            return;
        }
        // Ground collision handling.
        if (mainBodyChunk.ContactPoint.y < 0)
        {
            Vector2 groundRotation = Custom.DegToVec(90f); // Custom.DegToVec(90f * (facingRight ? 1 : -1));
            _rotation = Vector2.Lerp(_rotation, groundRotation, Random.value);
            rotVel *= Random.value;
        }
    }

    public void SetRotationVelocity()
    {
        // -- Ms7: Waht is this doing???
        // What this is doing is it's making the rotation velocity be set on thrown, or when falling off cliff,
        // But this is a janky method of doing so, I think both of these should be events instead.
        if (Vector2.Distance(mainBodyChunk.lastPos, mainBodyChunk.pos) > 5f && rotVel.magnitude < 7f)
        {
            rotVel += Custom.RNV() * (Mathf.Lerp(7f, 25f, Random.value) + mainBodyChunk.vel.magnitude * 2f);
            return;
        }
    }
}
