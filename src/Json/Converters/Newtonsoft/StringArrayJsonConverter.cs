namespace MRCustom.Json.Converters.Newtonsoft;

public class StringArrayJsonConverter : JsonConverter<string[]>
{
    public override string[] ReadJson(JsonReader reader, Type objectType, string[] existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JArray array = JArray.Load(reader);
        string[] strings = new string[array.Count];

        for (int i = 0; i < array.Count; i++)
        {
            JToken item = array[i];
            if (item.Type == JTokenType.String)
            {
                strings[i] = (string)item;
            }
            else
            {
                strings[i] = item.ToString(); // Fallback for non-string values
            }
        }

        return strings;
    }

    public override void WriteJson(JsonWriter writer, string[] value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        foreach (string str in value)
        {
            writer.WriteValue(str);
        }
        writer.WriteEndArray();
    }
}