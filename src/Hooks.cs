using MRCustom.Extensions.Items.Weapons;

namespace MRCustom;

public static class Hooks
{
    // Add hooks
    internal static void ApplyHooks()
    {
        ApplyRainWorldGameHooks();

        PlayerHooks.ApplyHooks();
        ApplyPlayerGraphicsHooks();
        SlugcatHandHooks.ApplyHooks();

        WeaponHooks.ApplyHooks();

        SparkHooks.ApplyHooks();
        StationaryEffectHooks.ApplyHooks();

        PoleMimicHooks.ApplyHooks();
        PoleMimicGraphicsHooks.ApplyHooks();

        ApplyRoomCameraHooks();
    }

    // Remove hooks
    internal static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        RemoveRainWorldGameHooks();

        PlayerHooks.RemoveHooks();
        RemovePlayerGraphicsHooks();
        SlugcatHandHooks.RemoveHooks();

        WeaponHooks.RemoveHooks();

        SparkHooks.RemoveHooks();
        StationaryEffectHooks.RemoveHooks();

        PoleMimicHooks.RemoveHooks();
        PoleMimicGraphicsHooks.RemoveHooks();

        RemoveRoomCameraHooks();
    }

    // RAIN WORLD GAME

    private static void ApplyRainWorldGameHooks()
    {
        On.RainWorldGame.ctor += RainWorldGameHooks.RainWorldGame_ctor;
    }

    private static void RemoveRainWorldGameHooks()
    {
        On.RainWorldGame.ctor -= RainWorldGameHooks.RainWorldGame_ctor;
    }

    // PLAYER GRAPHICS

    private static void ApplyPlayerGraphicsHooks()
    {
        On.PlayerGraphics.Update += PlayerGraphicsHooks.PlayerGraphics_Update;
    }

    private static void RemovePlayerGraphicsHooks()
    {
        On.PlayerGraphics.Update -= PlayerGraphicsHooks.PlayerGraphics_Update;
    }

    // ROOM CAMERA

    private static void ApplyRoomCameraHooks()
    {
        On.RoomCamera.DrawUpdate += RoomCameraHooks.RoomCamera_DrawUpdate;
    }

    private static void RemoveRoomCameraHooks()
    {
        On.RoomCamera.DrawUpdate -= RoomCameraHooks.RoomCamera_DrawUpdate;
    }

}