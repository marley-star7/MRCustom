using UnityEngine;
using RWCustom;

using MRCustom.Animations;

namespace MRCustom;

public static partial class Hooks
{
    // Add hooks
    public static void ApplyPlayerGraphicsHooks()
    {
        On.PlayerGraphics.Update += PlayerGraphics_Update;
    }

    // Remove hooks
    public static void RemovePlayerGraphicsHooks()
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