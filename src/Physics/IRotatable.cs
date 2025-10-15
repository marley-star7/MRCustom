using MRCustom.Modules.PhysicalObjects.Rotations;

namespace MRCustom.Physics;

public interface IRotatable
{
    public RotationModule RotationModule { get; }
}

public static class IRotatableExtensions
{
    public static Vector3 GetRotation(this IRotatable rotatable)
    {
        return rotatable.RotationModule.Rotation;
    }

    public static void SetRotation(this IRotatable rotatable, Vector3 rotation)
    {
        rotatable.RotationModule.Rotation = rotation;
    }

    public static void HardSetRotation(this IRotatable rotatable, Vector2 setRotation)
    {
        rotatable.RotationModule.SetRotation = setRotation;
    }

    public static float GetRotationDegrees(this IRotatable rotatable)
    {
        return Custom.VecToDeg(rotatable.RotationModule.Rotation);
    }

    public static void SetRotationDegrees(this IRotatable rotatable, float rotationDegrees)
    {
        rotatable.RotationModule.Rotation = Custom.DegToVec(rotationDegrees);
    }
}