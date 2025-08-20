namespace MRCustom.UI;

public struct ObjectIconSymbolProperties
{
    public string name;
    public string iconName = "MarError64";
    public float iconScale = 1.0f; // Default scale for the icon
    public Color color;

    public ObjectIconSymbolProperties(string name, string iconName, Color color, float iconScale)
    {
        this.name = name;
        this.iconName = iconName;
        this.color = color;
        this.iconScale = iconScale;
    }
}
