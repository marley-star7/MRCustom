namespace MRCustom.Modules.PhysicalObjects.Rotations;

public abstract class RotationModule : PhysicalObjectModule
{
    protected RotationModule(PhysicalObject owner) : base(owner, typeof(RotationModule))
    {
    }

    public abstract Vector2 Rotation { get; set; }
    public abstract Vector2 LastRotation { get; set; }

    public abstract Vector2? SetRotation { get; set; }

    /// <summary>
    /// Ran in the owner objects update function.
    /// </summary>
    public virtual void Update()
    {
        LastRotation = Rotation;

        if (SetRotation != null)
        {
            Rotation = SetRotation.Value;
            SetRotation = null;
            return;
        }

		LogicUpdate();
    }

    /// <summary>
    /// The actual place where you write your update.
    /// </summary>
    protected abstract void LogicUpdate();
}
