using Menu;
namespace MRCustom;

public static partial class Consts
{
    public static class LightningBoltTypes
    {
        /// <summary>
        /// Will slowly dissapear itself by removing lifeTime.
        /// </summary>
        public const int Temporary = 0;
        /// <summary>
        /// Will stay until manually removed.
        /// </summary>
        public const int Permanent = 1;
    }

    public static class Distances
    {
        public const int HalfTile = 5;
        public const int Tile = 10;
        public const int TwoTiles = 20;
    }

    public static class Player
    {
        /// <summary>
        /// The value used by the game to reset eating counter.
        /// </summary>
        public const int EatCounterResetValue = 30;
    }

    public static class SolidColors
    {
        public static readonly Color Blue = Custom.hexToColor("0101ff");
    }

    /// <summary>
    /// Used "ItemSymbol.ColorForItem(AbstractPhysicalObject.AbstractObjectType, int) : Color" To get most of these values.
    /// </summary>
    public static class IconColors
    {
        public static readonly Color MushroomWhite = Custom.hexToColor("ECECEC");
        public static readonly Color MediumGrey = Menu.Menu.MenuRGB(Menu.Menu.MenuColors.MediumGrey);
        public static readonly Color DangerRed = new Color(0.9019608f, 0.05490196f, 0.05490196f);
        public static readonly Color FirecrackerPlantRed = new Color(0.68235296f, 0.15686275f, 0.11764706f);
        public static readonly Color Red = new Color(0.68235296f, 0.15686275f, 0.11764706f);
        public static readonly Color LanternOrange = new Color(1f, 0.57254905f, 0.31764707f);
        public static readonly Color FlareBombBlue = Custom.hexToColor("b4a8f6");
        public static readonly Color WaterNutBlue = Color.blue;
    }
}
