namespace MRCustom.UI;

/// <summary>
/// Singleton for adding and storing all EntityTypeSymbolProperties
/// </summary>
public static class EntityTypeSymbolPropertiesManager
{
    private static Dictionary<EntityTypeDefinition, EntityTypeSymbolProperties> _entityTypeSymbolProperties = new();

    public static void AddEntityTypeSymbolProperties(EntityTypeDefinition entityTypeDefinition, EntityTypeSymbolProperties properties)
    {
        if (_entityTypeSymbolProperties.ContainsKey(entityTypeDefinition))
        {
            _entityTypeSymbolProperties[entityTypeDefinition] = properties;
        }
        else
        {
            _entityTypeSymbolProperties.Add(entityTypeDefinition, properties);
        }
    }

    public static void AddEntityTypeSymbolProperties(AbstractPhysicalObject.AbstractObjectType objectType, EntityTypeSymbolProperties properties)
    {
        AddEntityTypeSymbolProperties(new EntityTypeDefinition(objectType), properties);
    }

    public static EntityTypeSymbolProperties GetEntityTypeSymbolProperties(EntityTypeDefinition entityTypeDefinition)
    {
        if (_entityTypeSymbolProperties.TryGetValue(entityTypeDefinition, out var properties))
        {
            return properties;
        }
        else
        {
            // Return a default or error object UI properties if not found
            return new EntityTypeSymbolProperties("Unknown", "marError32", Color.magenta, 1f);
        }
    }

    public static EntityTypeSymbolProperties GetEntityTypeSymbolProperties(AbstractPhysicalObject.AbstractObjectType objectType)
    {
        return GetEntityTypeSymbolProperties(new EntityTypeDefinition(objectType));
    }
}
