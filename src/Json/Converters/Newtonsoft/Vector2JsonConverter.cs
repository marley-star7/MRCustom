namespace MRCustom.Json.Converters.Newtonsoft;

public class Vector2JsonConverter : JsonConverter<Vector2>
{
    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            // Handle array format: [x, y]
            JArray array = JArray.Load(reader);
            if (array.Count >= 2)
            {
                return new Vector2(
                    (float)array[0],
                    (float)array[1]
                );
            }
            return Vector2.zero;
        }
        else if (reader.TokenType == JsonToken.StartObject)
        {
            // Handle object format: {"x": 1.0, "y": 2.0}
            JObject obj = JObject.Load(reader);
            return new Vector2(
                (float)(obj["x"] ?? obj["X"] ?? 0f),
                (float)(obj["y"] ?? obj["Y"] ?? 0f)
            );
        }
        else
        {
            throw new JsonSerializationException($"Unexpected token type {reader.TokenType} when reading Vector2");
        }
    }

    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        // Write as array format: [x, y]
        writer.WriteStartArray();
        writer.WriteValue(value.x);
        writer.WriteValue(value.y);
        writer.WriteEndArray();

        // Alternatively, you could write as object format:
        // writer.WriteStartObject();
        // writer.WritePropertyName("x");
        // writer.WriteValue(value.x);
        // writer.WritePropertyName("y");
        // writer.WriteValue(value.y);
        // writer.WriteEndObject();
    }
}