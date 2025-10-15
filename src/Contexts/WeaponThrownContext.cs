namespace MRCustom.Contexts;

public class WeaponThrownContext
{
    public Creature thrownBy;
    public Vector2 thrownPos;
    public Vector2? firstFrameTraceFromPos;
    public IntVector2 throwDir;
    public float force;

    public WeaponThrownContext(
        Creature thrownBy,
        Vector2 thrownPos,
        Vector2? firstFrameTraceFromPos,
        IntVector2 throwDir,
        float force)
    {
        this.thrownBy = thrownBy;
        this.thrownPos = thrownPos;
        this.firstFrameTraceFromPos = firstFrameTraceFromPos;
        this.throwDir = throwDir;
        this.force = force;
    }
}
