namespace MRCustom.UI;

public static class ObjectIconSymbolPropertiesManager
{
    private static Dictionary<ObjectDefinition, ObjectIconSymbolProperties> _objectIconSymbolProperties = new();

    public static void AddObjectIconSymbolProperties(ObjectDefinition objectDefinition, ObjectIconSymbolProperties properties)
    {
        if (_objectIconSymbolProperties.ContainsKey(objectDefinition))
        {
            _objectIconSymbolProperties[objectDefinition] = properties;
        }
        else
        {
            _objectIconSymbolProperties.Add(objectDefinition, properties);
        }
    }

    public static void AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType objectType, ObjectIconSymbolProperties properties)
    {
        AddObjectIconSymbolProperties(new ObjectDefinition(objectType), properties);
    }

    public static ObjectIconSymbolProperties GetObjectIconSymbolProperties(ObjectDefinition objectDefinition)
    {
        if (_objectIconSymbolProperties.TryGetValue(objectDefinition, out var properties))
        {
            return properties;
        }
        else
        {
            // Return a default or error object UI properties if not found
            return new ObjectIconSymbolProperties("Unknown", "marError32", Color.magenta, 1f);
        }
    }

    public static ObjectIconSymbolProperties GetObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType objectType)
    {
        return GetObjectIconSymbolProperties(new ObjectDefinition(objectType));
    }
}
