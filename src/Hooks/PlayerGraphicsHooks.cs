using MRCustom.Animations;

namespace MRCustom.Hooks;

public static class PlayerGraphicsHooks
{
    // Add hooks
    internal static void ApplyHooks()
    {
        On.PlayerGraphics.Update += PlayerGraphics_Update;
    }

    // Remove hooks
    internal static void RemoveHooks()
    {
        On.PlayerGraphics.Update -= PlayerGraphics_Update;
    }

    private static void PlayerGraphics_Update(On.PlayerGraphics.orig_Update orig, PlayerGraphics self)
    {
        orig(self);

        var playerHandAnimationData = self.player.GetHandAnimationPlayer();

        if (playerHandAnimationData != null)
            playerHandAnimationData.GraphicsUpdate();
    }
}