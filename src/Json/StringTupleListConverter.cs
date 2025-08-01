namespace MRCustom.Json;

public class StringTupleListConverter : JsonConverter<List<(string, string)>>
{
    public override List<(string, string)> ReadJson(JsonReader reader, Type objectType, List<(string, string)> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var result = new List<(string, string)>();
        JArray outerArray = JArray.Load(reader);

        foreach (JToken innerArray in outerArray)
        {
            if (innerArray.Type == JTokenType.Array && innerArray.Count() >= 2)
            {
                result.Add((
                    innerArray[0]?.ToString(),  // First string
                    innerArray[1]?.ToString()   // Second string
                ));
            }
        }

        return result;
    }

    public override void WriteJson(JsonWriter writer, List<(string, string)> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (var (item1, item2) in value)
        {
            writer.WriteStartArray();
            writer.WriteValue(item1);
            writer.WriteValue(item2);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
}