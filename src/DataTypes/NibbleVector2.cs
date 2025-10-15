namespace MRCustom.DataTypes;

using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
/// <summary>
/// A 2D vector where each component is stored in a nibble (4 bits), allowing values from 0 to 15.
/// Uses a single byte for storage, making it memory efficient.
/// </summary>
public struct NibbleVector2 : IEquatable<NibbleVector2>
{
    [FieldOffset(0)]
    private byte _data;

    // 4 bits for x (0-15), 4 bits for y (0-15)
    private const byte X_MASK = 0x0F;      // 00001111
    private const byte Y_MASK = 0xF0;      // 11110000
    private const int Y_SHIFT = 4;

    private static NibbleVector2 _zeroVector = new NibbleVector2(0, 0);

    private static NibbleVector2 _oneVector = new NibbleVector2(1, 1);

    public static NibbleVector2 zero => _zeroVector;

    public static NibbleVector2 one => _oneVector;

    public static NibbleVector2 unitX => new NibbleVector2(1, 0);

    public static NibbleVector2 unitY => new NibbleVector2(0, 1);

    public NibbleVector2(byte x, byte y)
    {
        if (x > 15) throw new ArgumentOutOfRangeException(nameof(x), "x must be between 0 and 15");
        if (y > 15) throw new ArgumentOutOfRangeException(nameof(y), "y must be between 0 and 15");

        _data = (byte)(x & X_MASK | (y & X_MASK) << Y_SHIFT);
    }

    public byte x
    {
        get => (byte)(_data & X_MASK);
        set
        {
            if (value > 15) throw new ArgumentOutOfRangeException(nameof(value), "x must be between 0 and 15");
            _data = (byte)(_data & Y_MASK | value & X_MASK);
        }
    }

    public byte y
    {
        get => (byte)((_data & Y_MASK) >> Y_SHIFT);
        set
        {
            if (value > 15) throw new ArgumentOutOfRangeException(nameof(value), "y must be between 0 and 15");
            _data = (byte)(_data & X_MASK | (value & X_MASK) << Y_SHIFT);
        }
    }

    // Basic arithmetic operations
    public static NibbleVector2 operator +(NibbleVector2 a, NibbleVector2 b)
    {
        byte x = (byte)Math.Min(15, a.x + b.x);
        byte y = (byte)Math.Min(15, a.y + b.y);
        return new NibbleVector2(x, y);
    }

    public static NibbleVector2 operator -(NibbleVector2 a, NibbleVector2 b)
    {
        byte x = (byte)Math.Max(0, a.x - b.x);
        byte y = (byte)Math.Max(0, a.y - b.y);
        return new NibbleVector2(x, y);
    }

    public static NibbleVector2 operator *(NibbleVector2 a, byte scalar)
    {
        byte x = (byte)Math.Min(15, a.x * scalar);
        byte y = (byte)Math.Min(15, a.y * scalar);
        return new NibbleVector2(x, y);
    }

    // Comparison operators
    public static bool operator ==(NibbleVector2 left, NibbleVector2 right) => left.Equals(right);
    public static bool operator !=(NibbleVector2 left, NibbleVector2 right) => !left.Equals(right);

    // Utility methods
    public float Length() => Mathf.Sqrt(x * x + y * y);
    public float DistanceTo(NibbleVector2 other) => (this - other).Length();

    public override string ToString() => $"({x}, {y})";
    public override bool Equals(object obj) => obj is NibbleVector2 other && Equals(other);
    public bool Equals(NibbleVector2 other) => _data == other._data;
    public override int GetHashCode() => _data.GetHashCode();

    public static explicit operator UnityEngine.Vector2(NibbleVector2 nibbleVec)
    {
        return new UnityEngine.Vector2(nibbleVec.x, nibbleVec.y);
    }

    public static explicit operator NibbleVector2(UnityEngine.Vector2 vector)
    {
        return new NibbleVector2((byte)vector.x, (byte)vector.y);
    }

    // Conversion methods
    public UnityEngine.Vector2 ToVector2() => new UnityEngine.Vector2(x, y);
    public static NibbleVector2 FromVector2(UnityEngine.Vector2 vector) => new NibbleVector2((byte)vector.x, (byte)vector.y);

    public static explicit operator IntVector2(NibbleVector2 nibbleVec)
    {
        return new IntVector2(nibbleVec.x, nibbleVec.y);
    }

    public static explicit operator NibbleVector2(IntVector2 vector)
    {
        return new NibbleVector2((byte)vector.x, (byte)vector.y);
    }
}