namespace MRCustom.Effects;

/// <summary>
/// MS7: Base game sparks REQUIRE lizard graphics be sent as a parameter to get the lizard graphics spark effects,
/// Well NAH, I'm changing it so that you don't. Just by mimicking the lizardGraphics source stuff but replacing it with my own.
/// </summary>
public static class SparkHooks
{
    internal static void ApplyHooks()
    {
        On.Spark.Update += SparkHooks.Spark_Update;
        On.Spark.DrawSprites += SparkHooks.Spark_DrawSprites;
    }

    internal static void RemoveHooks()
    {
        On.Spark.Update -= SparkHooks.Spark_Update;
        On.Spark.DrawSprites -= SparkHooks.Spark_DrawSprites;
    }

    internal static void Spark_Update(On.Spark.orig_Update orig, Spark self, bool eu)
    {
        orig(self, eu);

        var selfMarData = self.GetMarSparkData();
        if (selfMarData.lizardShellColorGraphics != null && selfMarData.lizardShellColorGraphics.whiteFlicker < 5)
        {
            self.life -= 0.2f;
        }
    }

    internal static void Spark_DrawSprites(On.Spark.orig_DrawSprites orig, Spark self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        orig(self, sLeaser, rCam, timeStacker, camPos);

        var selfMarData = self.GetMarSparkData();
        if (selfMarData.lizardShellColorGraphics != null && selfMarData.lizardShellColorGraphics.whiteFlicker > 0)
        {
            sLeaser.sprites[0].color = selfMarData.lizardShellColorGraphics.ShellColor();
        }
    }
}
