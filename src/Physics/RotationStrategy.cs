namespace MRCustom.Physics;

public abstract class RotationStrategy
{
    public PhysicalObject owner;

    public bool facingRight;

    public Vector2 rotationTimeStacked;
    public float rotationTimeStackedDegrees => Custom.VecToDeg(rotationTimeStacked);

    public abstract void Update();

    public abstract void DrawSpritesUpdate(float timeStacker);
}
