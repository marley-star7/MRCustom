namespace MRCustom.Effects;

public class SparkData
{
    public WeakReference<Spark> sparkRef;

    public LizardEffectColorGraphics lizardShellColorGraphics;

    public SparkData(Spark spark)
    {
        this.sparkRef = new WeakReference<Spark>(spark);
    }
}

public static class SparkCraftingExtensions
{
    private static readonly ConditionalWeakTable<Spark, SparkData> conditionalWeakTable = new();

    public static SparkData GetMarSparkData(this Spark spark)
    {
        return conditionalWeakTable.GetValue(spark, _ => new SparkData(spark));
    }
}
