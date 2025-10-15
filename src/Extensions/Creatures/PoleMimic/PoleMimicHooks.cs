namespace MRCustom.Extensions;

internal static class PoleMimicHooks
{
    internal static void ApplyHooks()
    {
        On.PoleMimic.Update += PoleMimic_Update;
    }

    internal static void RemoveHooks()
    {
        On.PoleMimic.Update -= PoleMimic_Update;
    }

    /// <summary>
    /// Forces spears to fall off if they cut the pole plant so they do not float.
    /// </summary>
    /// <param name="orig"></param>
    /// <param name="self"></param>
    /// <param name="eu"></param>
    internal static void PoleMimic_Update(On.PoleMimic.orig_Update orig, PoleMimic self, bool eu)
    {
        orig(self, eu);

        var data = self.abstractCreature.GetMarAbstractPoleMimicData();

        if (data.spearList.Count == 0)
        {
            return;
        }

        List<Spear> tempList = new(data.spearList);
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i] == null)
            {
                data.spearList.Remove(tempList[i]);
                continue;
            }

            tempList[i].ChangeMode(Weapon.Mode.Free);
            data.spearList.Remove(tempList[i]);
        }
    }
}
