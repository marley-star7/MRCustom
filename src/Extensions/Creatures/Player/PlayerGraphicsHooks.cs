namespace MRCustom.Extensions;

internal static class PlayerGraphicsHooks
{
    internal static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);

        var playerHandAnimationData = self.player.GetHandAnimationPlayer();

        if (playerHandAnimationData != null)
            playerHandAnimationData.GraphicsUpdate();
    }
}