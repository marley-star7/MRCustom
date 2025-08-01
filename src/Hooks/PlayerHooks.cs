namespace MRCustom;

public static class PlayerHooks
{
    internal static void Player_NewRoom(On.Player.orig_NewRoom orig, Player self, Room newRoom)
    {
        orig(self, newRoom);
    }

    // Not sure why there is a difference between EatMeatUpdate and MaulingUpdate, nor do I know if this does anything, but just to be safe?
    internal static void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var playerHandAnimationData = self.GetHandAnimationPlayer();

        if (playerHandAnimationData != null)
            playerHandAnimationData.Update();
    }
}