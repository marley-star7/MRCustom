using MRCustom.Extensions.Creatures;
using SlugBase;

namespace MRCustom;

internal static class PlayerHooks
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

    internal static void Player_MovementUpdate(On.Player.orig_MovementUpdate orig, Player self, bool eu)
    {
        var selfMarData = self.GetPlayerMarData();

        if (!SlugBase.Features.PlayerFeatures.WalkSpeedMul.TryGet(self, out var baseWalkSpeed))
            Plugin.LogError($"{self.slugcatStats.name} character does not have a base walk speed defined in SlugBase, something has gone terribly wrong...");
        if (!SlugBase.Features.PlayerFeatures.ClimbSpeedMul.TryGet(self, out var basePoleClimbSpeed))
            Plugin.LogError($"{self.slugcatStats.name} character does not have a base pole climb speed defined in SlugBase, something has gone terribly wrong...");
        if (!SlugBase.Features.PlayerFeatures.TunnelSpeedMul.TryGet(self, out var baseCooridoorClimbSpeed))
            Plugin.LogError($"{self.slugcatStats.name} character does not have a base corridor climb speed defined in SlugBase, something has gone terribly wrong...");

        var finalRunSpeedLinearModifer = 1f;
        foreach (MarPlayerData.RunSpeedLinearModifier modifier in selfMarData.runSpeedLinearModifiers.dictionary.Values)
        {
            finalRunSpeedLinearModifer += modifier.value;
        }

        self.slugcatStats.runspeedFac = baseWalkSpeed[0] * finalRunSpeedLinearModifer;

        /*
        if (self.rollCounter > 15)
            self.EndRoll();
        */

        orig(self, eu);
    }
}