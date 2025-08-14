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

    // --- Color --- //

    public static bool TryParseColor(string colorString, out Color color)
    {
        color = Color.white; // Default value

        if (string.IsNullOrWhiteSpace(colorString))
            return false;

        // Remove whitespace and parentheses
        colorString = colorString.Trim().Trim('(', ')');

        try
        {
            // Hex format (#RRGGBB or #RRGGBBAA)
            if (colorString.StartsWith("#"))
            {
                return TryParseHex(colorString, out color);
            }

            // RGB or RGBA format (comma separated)
            if (colorString.Contains(","))
            {
                return TryParseRgb(colorString, out color);
            }

            // Named color (red, green, blue, etc.)
            return TryParseNamedColor(colorString, out color);
        }
        catch
        {
            return false;
        }
    }

    private static bool TryParseHex(string hexString, out Color color)
    {
        color = Color.white;
        hexString = hexString.Replace("#", "");

        try
        {
            switch (hexString.Length)
            {
                case 6: // RRGGBB
                    color.r = Convert.ToInt32(hexString.Substring(0, 2), 16) / 255f;
                    color.g = Convert.ToInt32(hexString.Substring(2, 2), 16) / 255f;
                    color.b = Convert.ToInt32(hexString.Substring(4, 2), 16) / 255f;
                    color.a = 1f;
                    return true;

                case 8: // RRGGBBAA
                    color.r = Convert.ToInt32(hexString.Substring(0, 2), 16) / 255f;
                    color.g = Convert.ToInt32(hexString.Substring(2, 2), 16) / 255f;
                    color.b = Convert.ToInt32(hexString.Substring(4, 2), 16) / 255f;
                    color.a = Convert.ToInt32(hexString.Substring(6, 2), 16) / 255f;
                    return true;

                default:
                    return false;
            }
        }
        catch
        {
            return false;
        }
    }

    private static bool TryParseRgb(string rgbString, out Color color)
    {
        color = Color.white;
        string[] components = rgbString.Split(',');

        try
        {
            switch (components.Length)
            {
                case 3: // R,G,B
                    color.r = float.Parse(components[0].Trim());
                    color.g = float.Parse(components[1].Trim());
                    color.b = float.Parse(components[2].Trim());
                    color.a = 1f;
                    return true;

                case 4: // R,G,B,A
                    color.r = float.Parse(components[0].Trim());
                    color.g = float.Parse(components[1].Trim());
                    color.b = float.Parse(components[2].Trim());
                    color.a = float.Parse(components[3].Trim());
                    return true;

                default:
                    return false;
            }
        }
        catch
        {
            return false;
        }
    }

    private static bool TryParseNamedColor(string name, out Color color)
    {
        // Built-in Unity colors
        var namedColors = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
        {
            {"red", Color.red},
            {"green", Color.green},
            {"blue", Color.blue},
            {"white", Color.white},
            {"black", Color.black},
            {"yellow", Color.yellow},
            {"cyan", Color.cyan},
            {"magenta", Color.magenta},
            {"gray", Color.gray},
            {"grey", Color.grey},
            {"clear", Color.clear}
        };

        return namedColors.TryGetValue(name.ToLower(), out color);
    }
}
