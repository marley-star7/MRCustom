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
}
