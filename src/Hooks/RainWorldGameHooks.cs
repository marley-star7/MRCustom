namespace MRCustom;

public static class RainWorldGameHooks
{
    internal static void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame rainWorldGame, ProcessManager manager)
    {
        orig(rainWorldGame, manager);

        if (Plugin.restartMode)
        {
            Hooks.ApplyHooks();

            Plugin.RainWorld_PostModsInit((_) => { }, rainWorldGame.rainWorld);
        }
    }
}