namespace MRCustom.Effects;

/// <summary>
/// Copies lizard graphics mimick code from StationaryEffect in base game, but using lizardColorGraphics.
/// </summary>
public static class StationaryEffectHooks
{
    internal static void ApplyHooks()
    {
        On.StationaryEffect.Update += StationaryEffect_Update;
        On.StationaryEffect.DrawSprites += StationaryEffect_DrawSprites;
    }

    internal static void RemoveHooks()
    {
        On.StationaryEffect.Update -= StationaryEffect_Update;
        On.StationaryEffect.DrawSprites -= StationaryEffect_DrawSprites;
    }

    internal static void StationaryEffect_Update(On.StationaryEffect.orig_Update orig, StationaryEffect self, bool eu)
    {
        var selfMarData = self.GetMarStationaryEffectData();
        if (selfMarData.lizardShellColorGraphics != null && selfMarData.lizardShellColorGraphics.whiteFlicker == 0)
        {
            self.life = 0;
        }

        orig(self, eu);
    }

    internal static void StationaryEffect_DrawSprites(On.StationaryEffect.orig_DrawSprites orig, StationaryEffect self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        orig(self, sLeaser, rCam, timeStacker, camPos);

        var selfMarData = self.GetMarStationaryEffectData();
        if (selfMarData.lizardShellColorGraphics != null && selfMarData.lizardShellColorGraphics.whiteFlicker > 0)
        {
            sLeaser.sprites[0].color = selfMarData.lizardShellColorGraphics.ShellColor();
        }
    }
}
