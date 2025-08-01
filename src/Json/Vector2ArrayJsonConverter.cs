namespace MRCustom.Json;

public class Vector2ArrayJsonConverter : JsonConverter<Vector2[]>
{
    public override Vector2[] ReadJson(JsonReader reader, Type objectType, Vector2[] existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JArray array = JArray.Load(reader);
        Vector2[] vectors = new Vector2[array.Count];

        for (int i = 0; i < array.Count; i++)
        {
            JToken item = array[i];
            if (item.Type == JTokenType.Array && item.Count() >= 2)
            {
                vectors[i] = new Vector2(
                    (float)item[0],  // X
                    (float)item[1]   // Y
                );
            }
        }

        return vectors;
    }

    public override void WriteJson(JsonWriter writer, Vector2[] value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (Vector2 vec in value)
        {
            writer.WriteStartArray();
            writer.WriteValue(vec.x);
            writer.WriteValue(vec.y);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
}