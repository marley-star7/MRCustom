namespace MRCustom.Math;

public static class MarMath
{
    /// <summary>
    /// Returns the sign value, but never as a zero.
    /// Instead will default to 1.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int NonzeroSign(float value)
    {
        var s = System.Math.Sign(value);
        if (s == 0)
            s = 1;
        return s;
    }

    /// <summary>
    /// Returns 'value' with the same sign as 'sign' (magnitude of 'value' remains unchanged).
    /// If 'value' is zero, returns 0 regardless of sign.
    /// </summary>
    public static int MatchSign(int value, int sign)
    {
        if (value == 0) return 0;  // Preserve zero
        return System.Math.Abs(value) * System.Math.Sign(sign);
    }
}
