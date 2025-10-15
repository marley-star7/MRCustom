namespace MRCustom.Effects;

public class StationaryEffectData
{
    public WeakReference<StationaryEffect> effectRef;

    public LizardEffectColorGraphics lizardShellColorGraphics;

    public StationaryEffectData(StationaryEffect spark)
    {
        this.effectRef = new WeakReference<StationaryEffect>(spark);
    }
}

public static class StationaryEffectExtensions
{
    private static readonly ConditionalWeakTable<StationaryEffect, StationaryEffectData> conditionalWeakTable = new();

    public static StationaryEffectData GetMarStationaryEffectData(this StationaryEffect spark)
    {
        return conditionalWeakTable.GetValue(spark, _ => new StationaryEffectData(spark));
    }
}
