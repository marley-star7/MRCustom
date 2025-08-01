namespace MRCustom.Json;

/// <summary>
/// Old outdated class for more manual conversions using rw's just string to dictionary json converter.
/// </summary>
public static class MRJson
{
    public static bool TryParseVector2Property(Dictionary<string, object> jsonData, string propertyName, out Vector2 result)
    {
        result = Vector2.zero;

        if (!jsonData.TryGetValue(propertyName, out var rawValue))
            return false; // Missing properties are treated as default values

        // Case 1: Direct Vector2 value
        if (rawValue is Vector2 vector2)
        {
            result = vector2;
            return true;
        }

        // Case 2: Dictionary format {x,y}
        if (rawValue is Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("x", out var xVal) && dict.TryGetValue("y", out var yVal)
                && TryConvertToFloat(xVal, out float xFloat) && TryConvertToFloat(yVal, out float yFloat))
            {
                result = new Vector2(xFloat, yFloat);
                return true;
            }
            Plugin.LogWarning($"Failed to parse Vector2 property of propertyName {propertyName}, attempting to parse as Dictionary format: " + "{x,y}");
            return false;
        }

        // Case 3: Array format [x,y]
        if (rawValue is List<object> list && list.Count >= 2
            && TryConvertToFloat(list[0], out float x) && TryConvertToFloat(list[1], out float y))
        {
            result = new Vector2(x, y);
            return true;
        }

        Plugin.LogWarning($"Failed to parse Vector2 property of propertyName {propertyName}");
        return false;
    }

    public static bool TryParseIntProperty(Dictionary<string, object> jsonData, string propertyName, out int result)
    {
        result = default;

        // Missing property is treated as default value
        if (!jsonData.TryGetValue(propertyName, out var rawValue))
        {
            Plugin.LogWarning($"Failed to parse Int property of propertyName {propertyName}");
            return false;
        }

        // Case 1: Direct int value
        if (rawValue is int intValue)
        {
            result = intValue;
            return true;
        }

        // Case 2: Other numeric types that can be converted
        try
        {
            result = Convert.ToInt32(rawValue);
            return true;
        }
        catch
        {
            Plugin.LogWarning($"Failed to parse Int property of propertyName {propertyName}");
            return false;
        }
    }

    public static bool TryParseFloatProperty(Dictionary<string, object> jsonData, string propertyName, out float result)
    {
        result = default;
        bool success = jsonData.TryGetValue(propertyName, out var rawValue) || TryConvertToFloat(rawValue, out result);

        if (!success)
            Plugin.LogWarning($"Failed to parse float property of propertyName {propertyName}");
        return success;
    }

    private static bool TryConvertToFloat(object value, out float result)
    {
        result = default;
        try
        {
            result = Convert.ToSingle(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool TryParseStringProperty(Dictionary<string, object> jsonData, string propertyName, out string result)
    {
        result = default;

        if (!jsonData.TryGetValue(propertyName, out var rawValue))
            return false; // Optional field

        switch (rawValue)
        {
            case string s:
                result = s;
                return true;
            case IConvertible c:
                result = c.ToString();
                return true;
            default:
                Plugin.LogWarning($"Failed to parse string property of propertyName {propertyName}");
                return false;
        }
    }

    public static bool TryParsePropertyAsList(Dictionary<string, object> jsonData, string propertyName, out List<object> result)
    {
        result = null;

        if (!jsonData.TryGetValue(propertyName, out var listProperty))
        {
            Plugin.LogWarning($"Failed parse of {propertyName}, could not find propertyName");
            return false;
        }

        if (listProperty is not List<object> listObj)
        {
            Plugin.LogWarning($"Failed parse of {propertyName}, json property is not a list");
            return false;
        }

        result = listObj;
        return true;
    }

    //
    // If either of these throws and you're wondering why:
    // ensure cctor for your custom ExtEnum is being called before any of these is called - typeof doesn't trigger cctor
    // ...
    // that's it ig, idk what else could throw as long as you aren't silly


    public static bool TryGetExtEnum<T>(this string self, out T result, bool ignoreCase = false) where T : ExtEnum<T>
    {
        if (ExtEnumBase.TryParse(typeof(T), self, ignoreCase, out ExtEnumBase value)
          && value is T converted)
        {
            result = converted;
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryGetExtEnum<T, TKey>(this Dictionary<TKey, string> self, TKey name, out T result) where T : ExtEnum<T>
    {
        if (self.TryGetValue(name, out string fieldName))
            return fieldName.TryGetExtEnum(out result, true);

        result = default;
        return false;
    }
}
