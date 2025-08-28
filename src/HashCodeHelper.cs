namespace MRCustom;

/// <summary>
/// Class to help with generating hash codes from multiple fields.
/// </summary>
public static class HashCodeHelper
{
    //-- Ms7: If you don't know what a hash code is, neither did I so I wrote it down:
    // Apparently, a hash code is a numerical value that represents an object.
    // This value is used in some data structures, (like hash tables) to quickly locate and access the object.
    // (As the int returned by GetHashCode() is always the same for objects with the same data, being calculated based off the types)

    // The "Arbitrary" numbers 17 and 23 are used because they are prime numbers, which helps in reducing hash collisions, as each multiplication is unique.
    // The "Unchecked" keyword is used to allow arithmetic overflow without throwing an exception.

    public static int Combine(params object[] objects)
    {
        unchecked
        {
            int hash = 17;
            foreach (var obj in objects)
            {
                hash = hash * 23 + (obj?.GetHashCode() ?? 0);
            }
            return hash;
        }
    }

    public static int Combine<T1>(T1 value1)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (value1?.GetHashCode() ?? 0);
            return hash;
        }
    }

    public static int Combine<T1, T2>(T1 value1, T2 value2)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (value1?.GetHashCode() ?? 0);
            hash = hash * 23 + (value2?.GetHashCode() ?? 0);
            return hash;
        }
    }

    public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (value1?.GetHashCode() ?? 0);
            hash = hash * 23 + (value2?.GetHashCode() ?? 0);
            hash = hash * 23 + (value3?.GetHashCode() ?? 0);
            return hash;
        }
    }

    // Add more overloads as needed

    public static int GetArrayHashCode<T>(T[] array)
    {
        if (array == null) return 0;
    
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + array.Length;
            foreach (var item in array)
            {
                hash = hash * 23 + (item?.GetHashCode() ?? 0);
            }
            return hash;
        }
    }
}
