
using System.Reflection;
using System.Reflection.Emit;

namespace MRCustom.Hooks;

public static class RainWorldGameHooks
{
    internal static void ApplyHooks()
    {
        On.RainWorldGame.ctor += RainWorldGame_ctor;
    }

    internal static void RemoveHooks()
    {
        On.RainWorldGame.ctor -= RainWorldGame_ctor;
    }

    private static void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame rainWorldGame, ProcessManager manager)
    {
        orig(rainWorldGame, manager);

        if (Plugin.restartMode)
        {
            Plugin.ApplyHooks();

            Plugin.RainWorld_PostModsInit((_) => { }, rainWorldGame.rainWorld);
        }
    }
}