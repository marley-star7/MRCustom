using MRCustom.Animations;

namespace MRCustom.Hooks;

public static class PlayerHooks
{
    // Add hooks
    internal static void ApplyHooks()
    {
        On.Player.NewRoom += Player_NewRoom;
        On.Player.Update += Player_Update;
    }

    // Remove hooks
    internal static void RemoveHooks()
    {
        On.Player.NewRoom -= Player_NewRoom;
        On.Player.Update -= Player_Update;
    }

    private static void Player_NewRoom(On.Player.orig_NewRoom orig, Player self, Room newRoom)
    {
        orig(self, newRoom);
    }

    // Not sure why there is a difference between EatMeatUpdate and MaulingUpdate, nor do I know if this does anything, but just to be safe?
    private static void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var playerHandAnimationData = self.GetHandAnimationPlayer();

        if (playerHandAnimationData != null)
            playerHandAnimationData.Update();
    }
}