namespace MRCustom.Modules.PhysicalObjects.Rotations;

public abstract class Vector3RotationModule : RotationModule
{
    protected Vector2 _rotation;
    protected Vector2 _lastRotation;

    protected Vector2? _setRotation;

    protected Vector3RotationModule(PhysicalObject owner) : base(owner)
    {
    }

    public override Vector2 Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }
    public override Vector2 LastRotation
    {
        get { return _lastRotation; }
        set { _lastRotation = value; }
    }

    public override Vector2? SetRotation
    {
        get { return _setRotation; }
        set { _setRotation = value; }
    }
}
