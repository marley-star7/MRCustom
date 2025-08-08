namespace MRCustom.Physics;

public interface IRotatable
{
    public Vector2 rotation { get; set; }
    public Vector2 lastRotation { get; set; }
}
