using UnityEngine;

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
}
