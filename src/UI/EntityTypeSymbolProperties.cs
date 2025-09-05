namespace MRCustom.UI;

/// <summary>
/// Stored information for getting the symbol and relevant properties associated with an entity type.
/// </summary>
public struct EntityTypeSymbolProperties
{
    public string name;
    public string spriteName = "marError64";
    public float iconScale = 1.0f; // Default scale for the icon
    public Color color;

    public EntityTypeSymbolProperties(string name, string spriteName, Color color, float iconScale)
    {
        this.name = name;
        this.spriteName = spriteName;
        this.color = color;
        this.iconScale = iconScale;
    }
}
