namespace MRCustom.Extensions.Creatures.PoleMimic;

public static class AbstractPoleMimicHooks
{
    internal static void ApplyHooks()
    {
        On.AbstractCreature.ctor += AbstractCreature_ctor;
    }

    internal static void RemoveHooks()
    {
        On.AbstractCreature.ctor -= AbstractCreature_ctor;
    }

    /// <summary>
    /// Adds the pole data.
    /// </summary>
    /// <param name="orig"></param>
    /// <param name="self"></param>
    /// <param name="world"></param>
    /// <param name="creatureTemplate"></param>
    /// <param name="realizedCreature"></param>
    /// <param name="pos"></param>
    /// <param name="ID"></param>
    private static void AbstractCreature_ctor(On.AbstractCreature.orig_ctor orig, AbstractCreature self, World world, CreatureTemplate creatureTemplate, Creature realizedCreature, WorldCoordinate pos, EntityID ID)
    {
        orig(self, world, creatureTemplate, realizedCreature, pos, ID);

        if (self == null || self.state == null || self.state.unrecognizedSaveStrings == null || creatureTemplate.type != CreatureTemplate.Type.PoleMimic)
        {
            return;
        }

        var data = self.GetMarAbstractPoleMimicData();
    }
}