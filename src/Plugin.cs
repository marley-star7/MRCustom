// TODO: figure out a way to do signal events.

using MRCustom.Events;
using MRCustom.UI;

namespace MRCustom;

// There are two types of dependencies:
// 1. BepInDependency.DependencyFlags.HardDependency - The other mod *MUST* be installed, and your mod cannot run without it. This ensures their mod loads before yours, preventing errors.
// 2. BepInDependency.DependencyFlags.SoftDependency - The other mod doesn't need to be installed, but if it is, it should load before yours.
//[BepInDependency("author.some_other_mods_guid", BepInDependency.DependencyFlags.HardDependency)]

[BepInPlugin(ID, NAME, VERSION)]

sealed class Plugin : BaseUnityPlugin
{
    public const string ID = "marley-star7.marcustom"; // This should be the same as the id in modinfo.json!
    public const string NAME = "MRCustom"; // This should be a human-readable version of your mod's name. This is used for log files and also displaying which mods get loaded. In general, it's a good idea to match this with your modinfo.json as well.
    public const string VERSION = "0.0.1"; // This follows semantic versioning. For more information, see https://semver.org/ - again, match what you have in modinfo.json

    public static bool isPostInit;
    public static bool restartMode = false;

    public static bool isSplitScreenCoopEnabled = false;

    public static bool improvedInputEnabled;
    public static int improvedInputVersion = 0;

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private static new ManualLogSource Logger;
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public void OnEnable()
    {
        Logger = base.Logger;

        On.RainWorld.OnModsInit += Extras.WrapInit(LoadPlugin);
        On.RainWorld.PostModsInit += RainWorld_PostModsInit;
        RegisterObjectIconSymbolProperties();

        Logger.LogInfo("MRCustom is loaded!");
    }

    private static void LoadPlugin(RainWorld rainWorld)
    {
        if (!restartMode)
        {
            Hooks.ApplyHooks();
            MREvents.ApplyEvents();
        }

        Futile.atlasManager.LoadAtlas("atlases/marError64");
        Futile.atlasManager.LoadAtlas("atlases/marNothing");
    }

    public void OnDisable()
    {
        if (restartMode)
        {
            Hooks.RemoveHooks();
            MREvents.RemoveEvents();
        }
    }

    internal static void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld rainWorld)
    {
        orig(rainWorld);

        try
        {
            if (Plugin.isPostInit)
                return;
            else
                Plugin.isPostInit = true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e.Message);
        }

        foreach (ModManager.Mod mod in ModManager.ActiveMods)
        {
            // if (mod.id == "crs") {
            if (mod.id == "henpemaz_splitscreencoop")
            {
                isSplitScreenCoopEnabled = true;
                continue;
            }
        }
    }

    private void RegisterObjectIconSymbolProperties()
    {
        // TODO: make interface for fisob properties for every item type to store information about the item, such as properties for armor, properties for bombs, etc.
        // TODO: then for base game objects that you can't add to, make an interface for the properties class that fisob uses, to inherit with your fisob properties, and then can make a new properties thing too.
        // TODO: maybe name them object statistics, and they use => to refer to the properties of the object, such as armor, damage, etc.


        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.Mushroom, new ObjectIconSymbolProperties("Mushroom", "Symbol_Mushroom", Custom.hexToColor("ECECEC"), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.KarmaFlower, new ObjectIconSymbolProperties("Karma Flower", "smallKarma9-9", Custom.hexToColor("bba557"), 0.5f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.SlimeMold, new ObjectIconSymbolProperties("Slime Mold", "Symbol_SlimeMold", Custom.hexToColor("ff9900"), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.SSOracleSwarmer, new ObjectIconSymbolProperties("SSOracleSwarmer", "Symbol_Neuron", Custom.hexToColor("FFFFFF"), 1f));
        //ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.GooieDuck, new ObjectIconSymbolProperties("GooieDuck", "Symbol_GooieDuck", new Color(0.44705883f, 0.9019608f, 0.76862746f), 1f));
        //ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.LillyPuck, new ObjectIconSymbolProperties("LillyPuck", "Symbol_LillyPuck", new Color(0.17058827f, 0.9619608f, 0.9986275f), 1f));
        //ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.GlowWeed, new ObjectIconSymbolProperties("GlowWeed", "Symbol_GlowWeed", new Color(0.94705886f, 1f, 0.26862746f), 1f));
        //ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.Seed, new ObjectIconSymbolProperties("Seed", "Symbol_Seed", Custom.hexToColor("fffcb8"), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.JellyFish, new ObjectIconSymbolProperties("Jellyfish", "Symbol_JellyFish", Color.grey, 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.FirecrackerPlant, new ObjectIconSymbolProperties("Cherrybomb", "Symbol_Firecracker", new Color(0.68235296f, 0.15686275f, 0.11764706f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.ScavengerBomb, new ObjectIconSymbolProperties("Grenade", "Symbol_StunBomb", new Color(0.9019608f, 0.05490196f, 0.05490196f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.FlareBomb, new ObjectIconSymbolProperties("Flashbang", "Symbol_FlashBomb", Custom.hexToColor("b4a8f6"), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.SporePlant, new ObjectIconSymbolProperties("Beehive", "Symbol_SporePlant", new Color(0.68235296f, 0.15686275f, 0.11764706f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.PuffBall, new ObjectIconSymbolProperties("Spore Plant", "Symbol_PuffBall", Color.grey, 1f));
        //ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.FireEgg, new ObjectIconSymbolProperties("FireEgg", "Symbol_FireEgg", new Color(1f, 0.47058824f, 0.47058824f), 1f));
        //ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.SingularityBomb, new ObjectIconSymbolProperties("SingularityBomb", "Symbol_Singularity", new Color(0.01961f, 0.6451f, 0.85f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.FlyLure, new ObjectIconSymbolProperties("FlyLure", "Symbol_FlyLure", new Color(0.6784314f, 0.26666668f, 0.21176471f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.BubbleGrass, new ObjectIconSymbolProperties("BubbleGrass", "Symbol_BubbleGrass", new Color(0.05490196f, 0.69803923f, 0.23529412f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.Lantern, new ObjectIconSymbolProperties("Lantern", "Symbol_Lantern", new Color(1f, 0.57254905f, 0.31764707f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.DataPearl, new ObjectIconSymbolProperties("Pearl", "Symbol_Pearl", Color.grey, 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.VultureMask, new ObjectIconSymbolProperties("Vulture Mask", "Kill_Vulture", Color.grey, 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.NeedleEgg, new ObjectIconSymbolProperties("Noodlefly Egg", "needleEggSymbol", new Color(0.5764706f, 0.16078432f, 0.2509804f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.OverseerCarcass, new ObjectIconSymbolProperties("Overseer Eye", "Kill_Overseer", Color.grey, 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.DangleFruit, new ObjectIconSymbolProperties("Dangle Fruit", "Symbol_DangleFruit", new Color(0f, 0f, 1f), 1f));

        /*
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.TubeWorm, new ObjectIconSymbolProperties("TubeWorm", "Kill_Tubeworm", new Color(0.05f, 0.3f, 0.7f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.Snail, new ObjectIconSymbolProperties("Snail", "Kill_Snail", Color.grey, 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.Fly, new ObjectIconSymbolProperties("Fly", "Kill_Bat", Color.grey, 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.SmallNeedleWorm, new ObjectIconSymbolProperties("SmallNeedleWorm", "Kill_SmallNeedleWorm", new Color(1f, 0.59607846f, 0.59607846f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.SmallCentipede, new ObjectIconSymbolProperties("SmallCentipede", "Kill_Centipede1", new Color(1f, 0.6f, 0f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.VultureGrub, new ObjectIconSymbolProperties("VultureGrub", "Kill_VultureGrub", new Color(0.83137256f, 0.7921569f, 0.43529412f), 1f));
        ObjectIconSymbolPropertiesManager.AddObjectIconSymbolProperties(AbstractPhysicalObject.AbstractObjectType.Hazer, new ObjectIconSymbolProperties("Hazer", "Kill_Hazer", new Color(0.21176471f, 0.7921569f, 0.3882353f), 1f));
        */
    }

    internal static void LogInfo(object ex) => Logger.LogInfo(ex);

    internal static void LogMessage(object ex) => Logger.LogMessage(ex);

    // -- Ms7: String prints are expensive!
    // So just incase we forget any #if's anywhere to encase debug logs to be for debug builds only to reduce hit on user performance.
    internal static void LogDebug(object ex)
    {
#if DEBUG
        Logger.LogDebug(ex);
#endif
    }

    internal static void LogWarning(object ex) => Logger.LogWarning(ex);

    internal static void LogError(object ex) => Logger.LogError(ex);

    internal static void LogFatal(object ex) => Logger.LogFatal(ex);
}