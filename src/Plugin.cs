// TODO: figure out a way to do signal events.

using MRCustom.Events;

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

    internal static void LogInfo(object ex) => Logger.LogInfo(ex);

    internal static void LogMessage(object ex) => Logger.LogMessage(ex);

    internal static void LogDebug(object ex) => Logger.LogDebug(ex);

    internal static void LogWarning(object ex) => Logger.LogWarning(ex);

    internal static void LogError(object ex) => Logger.LogError(ex);

    internal static void LogFatal(object ex) => Logger.LogFatal(ex);
}