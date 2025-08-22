namespace MRCustom.UI;

public static class ObjectIconSymbolPropertiesManager
{
    private static Dictionary<AbstractPhysicalObject.AbstractObjectType, ObjectIconSymbolProperties> _objectIconSymbolProperties = new();
    private static Dictionary<CreatureTemplate.Type, ObjectIconSymbolProperties> _creatureIconSymbolProperties = new();

    public static void AddObjectIconSymbolProperties(
        AbstractPhysicalObject.AbstractObjectType objectType,
        ObjectIconSymbolProperties properties)
    {
        if (_objectIconSymbolProperties.ContainsKey(objectType))
        {
            _objectIconSymbolProperties[objectType] = properties;
        }
        else
        {
            _objectIconSymbolProperties.Add(objectType, properties);
        }
    }

    public static ObjectIconSymbolProperties GetObjectIconSymbolProperties(
        AbstractPhysicalObject.AbstractObjectType objectType)
    {
        if (_objectIconSymbolProperties.TryGetValue(objectType, out var properties))
        {
            return properties;
        }
        else
        {
            // Return a default or error object UI properties if not found
            return new ObjectIconSymbolProperties("Unknown", "marError64", Color.red, 1f);
        }
    }
}
