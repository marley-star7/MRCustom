using RWCustom;
using UnityEngine;

using MRCustom.Animations;

namespace MRCustom;

public static partial class Hooks
{
    // Add hooks
    public static void ApplyPlayerHooks()
    {
        On.Player.NewRoom += Player_NewRoom;
        On.Player.Update += Player_Update;
    }

    // Remove hooks
    public static void RemovePlayerHooks()
    {
        On.Player.NewRoom -= Player_NewRoom;
        On.Player.Update -= Player_Update;
    }

    private static void Player_NewRoom(On.Player.orig_NewRoom orig, Player self, Room newRoom)
    {
        throw new NotImplementedException();
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