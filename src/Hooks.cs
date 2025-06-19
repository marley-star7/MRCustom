using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Fisobs.Core;
using BepInEx;

namespace MRCustom;

public static partial class Hooks
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
            ApplyHooks();

            Plugin.RainWorld_PostModsInit((_) => { }, self.rainWorld);
        }
    }

    // Add hooks
    public static void ApplyHooks()
    {
        ApplyPlayerHooks();
        ApplyPlayerGraphicsHooks();
    }

    // Remove hooks
    public static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        RemovePlayerHooks();
        RemovePlayerGraphicsHooks();
    }
}