namespace MRCustom.Json;

public class EnumToIntJsonConverter<TEnum> : JsonConverter where TEnum : struct, Enum
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(int);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        try
        {
            if (reader.TokenType == JsonToken.String)
            {
                string enumString = reader.Value.ToString();
                if (Enum.TryParse<TEnum>(enumString, true, out var enumValue))
                {
                    return Convert.ToInt32(enumValue);
                }
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                int intValue = Convert.ToInt32(reader.Value);
                if (Enum.IsDefined(typeof(TEnum), intValue))
                {
                    return intValue;
                }
            }
        }
        catch
        {
            // Fall through to default value
        }

        // Return default value (0) if parsing fails
        return Convert.ToInt32(default(TEnum));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is int intValue && Enum.IsDefined(typeof(TEnum), intValue))
        {
            writer.WriteValue(Enum.GetName(typeof(TEnum), intValue));
        }
        else
        {
            writer.WriteValue(Convert.ToInt32(default(TEnum)));
        }
    }
}