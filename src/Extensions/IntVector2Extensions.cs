namespace MRCustom.Extensions;

/// <summary>
/// Includes some static properties for IntVector2 similar to Vector2's
/// </summary>
public static class IntVector2Extensions
{
    // Ms7: As IntVector2's are structs I do believe this micro-optimization of having static ones might not even matter? as they are sent by variable anyways lol.

    public static IntVector2 zero = new IntVector2(0, 0);
    public static IntVector2 up = new IntVector2(0, 1);
    public static IntVector2 down = new IntVector2(0, 1);

    /// <summary>
    /// Gets the direction from an intVector to another.
    /// </summary>
    /// <param name="fromIntVector2"></param>
    /// <param name="toIntVector2"></param>
    /// <returns></returns>
    public static IntVector2 DirectionTo(this IntVector2 fromIntVector2, IntVector2 toIntVector2)
    {
        if (fromIntVector2 == toIntVector2)
        {
            return IntVector2Extensions.zero; // No direction if points are the same
        }

        int deltaX = toIntVector2.x - fromIntVector2.x;
        int deltaY = toIntVector2.y - fromIntVector2.y;

        // Normalize to unit direction (-1, 0, or 1 for each component)
        int dirX = System.Math.Sign(deltaX);
        int dirY = System.Math.Sign(deltaY);

        return new IntVector2(dirX, dirY);
    }

    /// <summary>
    /// Gets the cardinal direction (no diagonals) of two intVectors.
    /// </summary>
    /// <param name="fromIntVector2"></param>
    /// <param name="toIntVector2"></param>
    /// <returns></returns>
    public static IntVector2 CardinalDirectionTo(this IntVector2 fromIntVector2, IntVector2 toIntVector2)
    {
        int deltaX = toIntVector2.x - fromIntVector2.x;
        int deltaY = toIntVector2.y - fromIntVector2.y;

        // Prefer horizontal movement if distances are equal
        if (System.Math.Abs(deltaX) >= System.Math.Abs(deltaY))
        {
            return new IntVector2(System.Math.Sign(deltaX), 0);
        }
        else
        {
            return new IntVector2(0, System.Math.Sign(deltaY));
        }
    }
}
