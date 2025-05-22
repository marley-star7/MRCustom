/*
using RWCustom;
using UnityEngine;

using MRCustom.Animations;

namespace MRCustom.Hooks;

public static partial class Hooks
{
    // Add hooks
    public static void ApplyPlayerHooks()
    {
        On.Player.Update += Player_Update;
    }

    // Remove hooks
    public static void RemovePlayerHooks()
    {
        On.Player.Update -= Player_Update;
    }

    // Not sure why there is a difference between EatMeatUpdate and MaulingUpdate, nor do I know if this does anything, but just to be safe?
    private static void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
    {
        orig(self, eu);

        var playerHandAnimationData = self.GetHandAnimationData();

        if (playerHandAnimationData != null)
            playerHandAnimationData.Update();
    }
}
*/
