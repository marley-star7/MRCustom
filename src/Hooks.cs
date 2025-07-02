namespace MRCustom.Hooks;

public static class Hooks
{
    private static void ApplyRainWorldGameHooks()
    {
        On.RainWorldGame.ctor += RainWorldGame_ctor;
    }

    private static void RemoveRainWorldGameHooks()
    {
        On.RainWorldGame.ctor -= RainWorldGame_ctor;
    }

    private static void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager)
    {
        if (Plugin.restartMode)
        {
            Plugin.ApplyHooks();

            Plugin.RainWorld_PostModsInit((_) => { }, self.rainWorld);
        }
    }
}