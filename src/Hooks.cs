namespace MRCustom;

public static class Hooks
{
    // Add hooks
    internal static void ApplyHooks()
    {
        ApplyRainWorldGameHooks();

        ApplyPlayerHooks();
        ApplyPlayerGraphicsHooks();

        ApplyRoomCameraHooks();
    }

    // Remove hooks
    internal static void RemoveHooks()
    {
        On.RainWorld.PostModsInit -= Plugin.RainWorld_PostModsInit;

        RemoveRainWorldGameHooks();

        RemovePlayerHooks();
        RemovePlayerGraphicsHooks();

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

    // PLAYER

    private static void ApplyPlayerHooks()
    {
        On.Player.NewRoom += PlayerHooks.Player_NewRoom;
        On.Player.Update += PlayerHooks.Player_Update;
    }

    private static void RemovePlayerHooks()
    {
        On.Player.NewRoom -= PlayerHooks.Player_NewRoom;
        On.Player.Update -= PlayerHooks.Player_Update;
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