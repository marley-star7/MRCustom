namespace MRCustom.Extensions;

public static class MarColorExtensions
{
    public static bool TryParse(string colorString, out Color color)
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
        var namedColors = new System.Collections.Generic.Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
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