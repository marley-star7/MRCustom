namespace MRCustom.DataTypes;

using System;
using System.Runtime.InteropServices;

using UnityEngine;

[StructLayout(LayoutKind.Explicit)]
/// <summary>
/// A 2D vector where each component is stored in a signed nibble (4 bits), allowing values from -7 to 7.
/// Uses the -8 value as a special flag. Uses a single byte for storage, making it memory efficient.
/// </summary>
public struct SNibbleVector2 : IEquatable<SNibbleVector2>
{
    [FieldOffset(0)]
    private byte _data;

    // 4 bits for x (-7 to 7), 4 bits for y (-7 to 7)
    // The -8 value (0b1000) is used as a special flag
    private const byte X_MASK = 0x0F;      // 00001111
    private const byte Y_MASK = 0xF0;      // 11110000
    private const int Y_SHIFT = 4;

    // Valid range for normal values
    private const sbyte MIN_VALUE = -7;
    private const sbyte MAX_VALUE = 7;

    // Special flag value
    private const sbyte SPECIAL_FLAG = -8;
    private const byte SPECIAL_FLAG_BITS = 0x08; // 0b1000

    private static SNibbleVector2 _zeroVector = new SNibbleVector2(0, 0);
    private static SNibbleVector2 _oneVector = new SNibbleVector2(1, 1);
    private static SNibbleVector2 _unitXVector = new SNibbleVector2(1, 0);
    private static SNibbleVector2 _unitYVector = new SNibbleVector2(0, 1);
    private static SNibbleVector2 _leftVector = new SNibbleVector2(-1, 0);
    private static SNibbleVector2 _rightVector = new SNibbleVector2(1, 0);
    private static SNibbleVector2 _upVector = new SNibbleVector2(0, 1);
    private static SNibbleVector2 _downVector = new SNibbleVector2(0, -1);

    public static SNibbleVector2 zero => _zeroVector;
    public static SNibbleVector2 one => _oneVector;
    public static SNibbleVector2 unitX => _unitXVector;
    public static SNibbleVector2 unitY => _unitYVector;
    public static SNibbleVector2 left => _leftVector;
    public static SNibbleVector2 right => _rightVector;
    public static SNibbleVector2 up => _upVector;
    public static SNibbleVector2 down => _downVector;

    // Special values using the -8 flag
    public static SNibbleVector2 invalid => new SNibbleVector2(SPECIAL_FLAG, SPECIAL_FLAG);
    public static SNibbleVector2 infinity => new SNibbleVector2(SPECIAL_FLAG, 0);
    public static SNibbleVector2 negativeInfinity => new SNibbleVector2(0, SPECIAL_FLAG);

    public SNibbleVector2(sbyte x, sbyte y)
    {
        if ((x < MIN_VALUE || x > MAX_VALUE) && x != SPECIAL_FLAG)
            throw new ArgumentOutOfRangeException(nameof(x), $"x must be between {MIN_VALUE} and {MAX_VALUE}, or {SPECIAL_FLAG} for special values");
        if ((y < MIN_VALUE || y > MAX_VALUE) && y != SPECIAL_FLAG)
            throw new ArgumentOutOfRangeException(nameof(y), $"y must be between {MIN_VALUE} and {MAX_VALUE}, or {SPECIAL_FLAG} for special values");

        // Convert signed to unsigned nibble (two's complement, but -8 is reserved for special flags)
        byte xNibble = (byte)(x & X_MASK);
        byte yNibble = (byte)(y & X_MASK);
        _data = (byte)(xNibble | (yNibble << Y_SHIFT));
    }

    public sbyte x
    {
        get
        {
            byte xNibble = (byte)(_data & X_MASK);
            // Convert unsigned nibble to signed
            return xNibble == SPECIAL_FLAG_BITS ? SPECIAL_FLAG : (sbyte)((xNibble > 7) ? (xNibble - 16) : xNibble);
        }
        set
        {
            if ((value < MIN_VALUE || value > MAX_VALUE) && value != SPECIAL_FLAG)
                throw new ArgumentOutOfRangeException(nameof(value), $"x must be between {MIN_VALUE} and {MAX_VALUE}, or {SPECIAL_FLAG} for special values");

            byte xNibble = (byte)(value & X_MASK);
            _data = (byte)((_data & Y_MASK) | xNibble);
        }
    }

    public sbyte y
    {
        get
        {
            byte yNibble = (byte)((_data & Y_MASK) >> Y_SHIFT);
            // Convert unsigned nibble to signed
            return yNibble == SPECIAL_FLAG_BITS ? SPECIAL_FLAG : (sbyte)((yNibble > 7) ? (yNibble - 16) : yNibble);
        }
        set
        {
            if ((value < MIN_VALUE || value > MAX_VALUE) && value != SPECIAL_FLAG)
                throw new ArgumentOutOfRangeException(nameof(value), $"y must be between {MIN_VALUE} and {MAX_VALUE}, or {SPECIAL_FLAG} for special values");

            byte yNibble = (byte)(value & X_MASK);
            _data = (byte)((_data & X_MASK) | (yNibble << Y_SHIFT));
        }
    }

    // Properties to check for special values
    public bool isInvalid => x == SPECIAL_FLAG && y == SPECIAL_FLAG;
    public bool hasInfinityX => x == SPECIAL_FLAG;
    public bool hasInfinityY => y == SPECIAL_FLAG;
    public bool hasAnySpecial => x == SPECIAL_FLAG || y == SPECIAL_FLAG;
    public bool isValid => !hasAnySpecial;

    // Basic arithmetic operations with special value handling
    public static SNibbleVector2 operator +(SNibbleVector2 a, SNibbleVector2 b)
    {
        if (a.hasAnySpecial || b.hasAnySpecial)
        {
            // Special handling for operations with special values
            if (a.isInvalid || b.isInvalid) return invalid;
            if (a.hasInfinityX || b.hasInfinityX) return infinity;
            if (a.hasInfinityY || b.hasInfinityY) return negativeInfinity;
        }

        sbyte x = (sbyte)Mathf.Clamp(a.x + b.x, MIN_VALUE, MAX_VALUE);
        sbyte y = (sbyte)Mathf.Clamp(a.y + b.y, MIN_VALUE, MAX_VALUE);
        return new SNibbleVector2(x, y);
    }

    public static SNibbleVector2 operator -(SNibbleVector2 a, SNibbleVector2 b)
    {
        if (a.hasAnySpecial || b.hasAnySpecial)
        {
            // Special handling for operations with special values
            if (a.isInvalid || b.isInvalid) return invalid;
            if (a.hasInfinityX || b.hasInfinityX) return infinity;
            if (a.hasInfinityY || b.hasInfinityY) return negativeInfinity;
        }

        sbyte x = (sbyte)Mathf.Clamp(a.x - b.x, MIN_VALUE, MAX_VALUE);
        sbyte y = (sbyte)Mathf.Clamp(a.y - b.y, MIN_VALUE, MAX_VALUE);
        return new SNibbleVector2(x, y);
    }

    public static SNibbleVector2 operator *(SNibbleVector2 a, sbyte scalar)
    {
        if (a.hasAnySpecial)
        {
            if (a.isInvalid) return invalid;
            if (a.hasInfinityX) return infinity;
            if (a.hasInfinityY) return negativeInfinity;
        }

        sbyte x = (sbyte)Mathf.Clamp(a.x * scalar, MIN_VALUE, MAX_VALUE);
        sbyte y = (sbyte)Mathf.Clamp(a.y * scalar, MIN_VALUE, MAX_VALUE);
        return new SNibbleVector2(x, y);
    }

    public static SNibbleVector2 operator -(SNibbleVector2 a)
    {
        if (a.hasAnySpecial)
        {
            if (a.isInvalid) return invalid;
            if (a.hasInfinityX) return negativeInfinity;
            if (a.hasInfinityY) return infinity;
        }

        return new SNibbleVector2((sbyte)-a.x, (sbyte)-a.y);
    }

    // Comparison operators with special value handling
    public static bool operator ==(SNibbleVector2 left, SNibbleVector2 right) => left.Equals(right);
    public static bool operator !=(SNibbleVector2 left, SNibbleVector2 right) => !left.Equals(right);

    // Utility methods
    public float Length()
    {
        if (hasAnySpecial) return float.NaN;
        return Mathf.Sqrt(x * x + y * y);
    }

    public float DistanceTo(SNibbleVector2 other)
    {
        if (hasAnySpecial || other.hasAnySpecial) return float.NaN;
        return (this - other).Length();
    }

    public SNibbleVector2 Clamped() => new SNibbleVector2(
        (sbyte)Mathf.Clamp(x, MIN_VALUE, MAX_VALUE),
        (sbyte)Mathf.Clamp(y, MIN_VALUE, MAX_VALUE)
    );

    public override string ToString()
    {
        if (isInvalid) return "SNibbleVector2(Invalid)";
        if (hasInfinityX) return "SNibbleVector2(Infinity)";
        if (hasInfinityY) return "SNibbleVector2(NegativeInfinity)";
        return $"SNibbleVector2({x}, {y})";
    }

    public override bool Equals(object obj) => obj is SNibbleVector2 other && Equals(other);
    public bool Equals(SNibbleVector2 other) => _data == other._data;
    public override int GetHashCode() => _data.GetHashCode();

    public static explicit operator UnityEngine.Vector2(SNibbleVector2 nibbleVec)
    {
        return new UnityEngine.Vector2(nibbleVec.x, nibbleVec.y);
    }

    public static explicit operator SNibbleVector2(UnityEngine.Vector2 vector)
    {
        return new SNibbleVector2((sbyte)vector.x, (sbyte)vector.y);
    }

    // Conversion methods
    public UnityEngine.Vector2 ToVector2()
    {
        if (hasAnySpecial) return new UnityEngine.Vector2(float.NaN, float.NaN);
        return new UnityEngine.Vector2(x, y);
    }

    public static SNibbleVector2 FromVector2(UnityEngine.Vector2 vector)
    {
        if (float.IsInfinity(vector.x) && vector.x > 0) return infinity;
        if (float.IsInfinity(vector.x) && vector.x < 0) return negativeInfinity;
        if (float.IsNaN(vector.x) || float.IsNaN(vector.y)) return invalid;

        return new SNibbleVector2((sbyte)vector.x, (sbyte)vector.y).Clamped();
    }

    public static explicit operator IntVector2(SNibbleVector2 nibbleVec)
    {
        return new IntVector2(nibbleVec.x, nibbleVec.y);
    }

    public static explicit operator SNibbleVector2(IntVector2 vector)
    {
        return new SNibbleVector2((sbyte)vector.x, (sbyte)vector.y);
    }
}