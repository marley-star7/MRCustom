namespace MRCustom.Extensions;

internal static class SlugcatHandHooks
{
    internal static void ApplyHooks()
    {
        On.SlugcatHand.EngageInMovement += SlugcatHand_EngageInMovement;
        On.SlugcatHand.Update += SlugcatHand_Update;
    }

    internal static void RemoveHooks()
    {
        On.SlugcatHand.EngageInMovement -= SlugcatHand_EngageInMovement;
        On.SlugcatHand.Update -= SlugcatHand_Update;
    }

    public static bool IsGrabbedDonned(in SlugcatHand hand)
    {
        Player scug = hand.owner.owner as Player;

        if (scug.grasps[hand.limbNumber] != null
            && scug.grasps[hand.limbNumber].grabbed.TryGetModule<DonnableMaskItemModule>(out var donnableMask)
            && donnableMask.donned > 0)
        {
            return true;
        }

        return false;
    }

    internal static bool SlugcatHand_EngageInMovement(On.SlugcatHand.orig_EngageInMovement orig, SlugcatHand self)
    {
        Player scug = self.owner.owner as Player;

        if (IsGrabbedDonned(self))
        {
            return true;
        }

        return orig(self);
    }

    internal static void SlugcatHand_Update(On.SlugcatHand.orig_Update orig, SlugcatHand self)
    {
        Player scug = self.owner.owner as Player;

        orig(self);

        if (IsGrabbedDonned(self))
        {
            var donnableMaskModule = scug.grasps[self.limbNumber].grabbed.GetModule<DonnableMaskItemModule>();

            /*
            self.mode = donnableMaskModule.slugcatHandMode;
            if (donnableMaskModule.slugcatHandMode == SlugcatHand.Mode.HuntAbsolutePosition)
            {
                self.absoluteHuntPos = donnableMaskModule.donnedHandAbsoluteHuntPos;
            }
            else if (donnableMaskModule.slugcatHandMode == SlugcatHand.Mode.HuntRelativePosition)
            {
                self.relativeHuntPos = donnableMaskModule.donnedHandRelativeHuntPos;
            }
            */
        }
    }
}
