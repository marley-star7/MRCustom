namespace MRCustom.Json;

public class StringTupleObjectConverter : JsonConverter<List<(string, string)>>
{
    private readonly string _firstPropertyName;
    private readonly string _secondPropertyName;

    public StringTupleObjectConverter(string firstProperty = "key", string secondProperty = "value")
    {
        _firstPropertyName = firstProperty;
        _secondPropertyName = secondProperty;
    }

    public override List<(string, string)> ReadJson(
        JsonReader reader,
        Type objectType,
        List<(string, string)> existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var result = new List<(string, string)>();
        JArray array = JArray.Load(reader);

        foreach (JObject obj in array.Children<JObject>())
        {
            result.Add((
                obj[_firstPropertyName]?.ToString(),
                obj[_secondPropertyName]?.ToString()
            ));
        }

        return result;
    }

    public override void WriteJson(
        JsonWriter writer,
        List<(string, string)> value,
        JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (var (item1, item2) in value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(_firstPropertyName);
            writer.WriteValue(item1);
            writer.WritePropertyName(_secondPropertyName);
            writer.WriteValue(item2);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();
    }
}