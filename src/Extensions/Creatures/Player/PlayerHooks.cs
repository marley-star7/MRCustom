using ImprovedInput;

namespace MRCustom.Extensions;

internal static class PlayerHooks
{
    internal static void ApplyHooks()
    {
        On.Player.NewRoom += PlayerHooks.Player_NewRoom;
        On.Player.Update += PlayerHooks.Player_Update;
        On.Player.Grabability += Player_Grabability;
    }

    internal static void RemoveHooks()
    {
        On.Player.NewRoom -= PlayerHooks.Player_NewRoom;
        On.Player.Update -= PlayerHooks.Player_Update;
        On.Player.Grabability -= Player_Grabability;
    }

    internal static void Player_NewRoom(On.Player.orig_NewRoom orig, Player self, Room newRoom)
    {
        orig(self, newRoom);
    }

    // Not sure why there is a difference between EatMeatUpdate and MaulingUpdate, nor do I know if this does anything, but just to be safe?
    internal static void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
    {
        var playerMarData = self.GetMarPlayerData();
        playerMarData.lastAnimation = self.animation;

        orig(self, eu);

        var playerHandAnimationData = self.GetHandAnimationPlayer();

        if (playerHandAnimationData != null)
            playerHandAnimationData.Update();

        OverridePlayerPressingGrabInputItemModule grabInputItemModule;
        if (self.grasps[0] != null 
            && (
                self.grasps[0].grabbed.TryGetModule<OverridePlayerPressingGrabInputItemModule>(out grabInputItemModule) // Check primary first for one.
                || self.grasps[1] != null && self.grasps[1].grabbed.TryGetModule<OverridePlayerPressingGrabInputItemModule>(out grabInputItemModule) // Then secondary if it exists,
            ))
        {
            if (self.JustPressed(PlayerKeybind.Grab))
            {
                grabInputItemModule.PlayerJustPressedGrabUpdate(self);
            }
            else if (self.IsPressed(PlayerKeybind.Grab)) // If Holding Grab
            {
                grabInputItemModule.PlayerPressingGrabUpdate(self);
            }
            else if (self.JustReleased(PlayerKeybind.Grab))
            {
                grabInputItemModule.PlayerJustReleasedGrabUpdate(self);
            }
            else
            {
                grabInputItemModule.PlayerNotPressingGrabUpdate(self);
            }
        }
    }

    internal static Player.ObjectGrabability Player_Grabability(On.Player.orig_Grabability orig, Player self, PhysicalObject obj)
    {
        var origResult = orig(self, obj);
        
        // If they got a can't grab, check ours to see if we can instead.
        if (origResult == Player.ObjectGrabability.CantGrab)
        {
            if (obj is Creature && !(obj as Creature).Template.smallCreature && ((obj as Creature).dead || SlugcatStatsExtensions.SlugcatCanPickupStunnedLargeCreatures(self.SlugCatClass) && self.dontGrabStuff < 1 && obj != self && !(obj as Creature).Consious))
            {
                return Player.ObjectGrabability.Drag;
            }
        }

        return origResult;
    }

    internal static void Player_MovementUpdate(On.Player.orig_MovementUpdate orig, Player self, bool eu)
    {
        var selfMarData = self.GetMarPlayerData();

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