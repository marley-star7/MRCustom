namespace MRCustom.Json.Converters.Newtonsoft;

public class HexadacimalToUnityColorConverter : JsonConverter<Color>
{
    public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
    {
        // Convert Color to hex string (RRGGBBAA format)
        string hex = ColorUtility.ToHtmlStringRGBA(value);
        writer.WriteValue($"#{hex}");
    }

    public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string hexString = reader.Value?.ToString();

            if (string.IsNullOrEmpty(hexString))
                return default;

            // Remove # if present
            if (hexString.StartsWith("#"))
                hexString = hexString.Substring(1);

            // Handle different hex formats
            if (hexString.Length == 6) // RRGGBB
            {
                hexString += "FF"; // Add alpha channel
            }
            else if (hexString.Length == 3) // RGB
            {
                // Expand to RRGGBBAA
                hexString = $"{hexString[0]}{hexString[0]}{hexString[1]}{hexString[1]}{hexString[2]}{hexString[2]}FF";
            }
            else if (hexString.Length == 8) // RRGGBBAA
            {
                // Already correct format
            }
            else
            {
                throw new JsonSerializationException($"Invalid hex color format: {hexString}");
            }

            if (ColorUtility.TryParseHtmlString($"#{hexString}", out Color color))
            {
                return color;
            }
            else
            {
                throw new JsonSerializationException($"Could not parse hex color: {hexString}");
            }
        }
        else if (reader.TokenType == JsonToken.Null)
        {
            return default;
        }

        throw new JsonSerializationException($"Unexpected token type {reader.TokenType} when parsing color.");
    }
}
