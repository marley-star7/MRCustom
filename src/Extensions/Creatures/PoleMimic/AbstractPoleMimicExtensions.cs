namespace MRCustom.Extensions;

public class MarAbstractPoleMimicData
{
    public int cutAppendage = -1;
    /// <summary>
    /// Useless unless there is a mod which lets pole mimics cheat death
    /// </summary>
    //public int cutAppendageCycle = -1;

    /// <summary>
    /// Used to store Spears that cut the pole mimic so they are then forced out of the pole mimic to prevent them floating
    /// </summary>
    public List<Spear> spearList = new();
}

public static class MarPoleMimicExtensions
{
    public static readonly ConditionalWeakTable<AbstractCreature, MarAbstractPoleMimicData> conditionalWeakTable = new();

    public static MarAbstractPoleMimicData GetMarAbstractPoleMimicData(this AbstractCreature abstractPoleMimic) => conditionalWeakTable.GetValue(abstractPoleMimic, _ => new MarAbstractPoleMimicData());
}