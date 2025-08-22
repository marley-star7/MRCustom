namespace MRCustom.UI;

public struct ObjectIconSymbolProperties
{
    public string name;
    public string spriteName = "marError64";
    public float iconScale = 1.0f; // Default scale for the icon
    public Color color;

    public ObjectIconSymbolProperties(string name, string spriteName, Color color, float iconScale)
    {
        this.name = name;
        this.spriteName = spriteName;
        this.color = color;
        this.iconScale = iconScale;
    }
}
