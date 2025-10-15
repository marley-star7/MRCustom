namespace MRCustom.Extensions;

internal static class PoleMimicGraphicsHooks
{
    internal static void ApplyHooks()
    {
        On.PoleMimicGraphics.DrawSprites += PoleMimicGraphics_DrawSprites;
        On.PoleMimicGraphics.PoleMimicRopeGraphics.DrawSprite += PoleMimicRopeGraphics_DrawSprite;
    }

    internal static void RemoveHooks()
    {
        On.PoleMimicGraphics.DrawSprites -= PoleMimicGraphics_DrawSprites;
        On.PoleMimicGraphics.PoleMimicRopeGraphics.DrawSprite -= PoleMimicRopeGraphics_DrawSprite;
    }

    internal static void PoleMimicGraphics_DrawSprites(On.PoleMimicGraphics.orig_DrawSprites orig, PoleMimicGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        orig(self, sLeaser, rCam, timeStacker, camPos);
        var abstractData = self.pole.abstractCreature.GetMarAbstractPoleMimicData();

        if (abstractData.cutAppendage == -1)
        {
            return;
        }

        //Hides all the Leafs (hopefully temporary)

        for (int i = 0; i < self.leafPairs; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                sLeaser.sprites[self.LeafSprite(i, j)].isVisible = false;

                if (i < self.decoratedLeafPairs)
                {
                    sLeaser.sprites[self.LeafDecorationSprite(i, j)].isVisible = false;
                }
            }
        }
    }

    internal static void PoleMimicRopeGraphics_DrawSprite(On.PoleMimicGraphics.PoleMimicRopeGraphics.orig_DrawSprite orig, PoleMimicGraphics.PoleMimicRopeGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        orig(self, sLeaser, rCam, timeStacker, camPos);
        var abstractData = self.owner.pole.abstractCreature.GetMarAbstractPoleMimicData();

        if (abstractData.cutAppendage == -1)
        {
            return;
        }

        float percentage = (float)abstractData.cutAppendage / (float)self.owner.pole.tentacle.segments.Count;

        int start = Mathf.RoundToInt(self.segments.Length * percentage) + 1;

        Vector2 vector = Vector2.Lerp(self.segments[start].lastPos, self.segments[start].pos, timeStacker);

        //Hides the PolePlant Segments
        for (int j = start - 1; j < self.segments.Length; j++)
        {
            (sLeaser.sprites[0] as TriangleMesh).MoveVertice(j * 4, vector - camPos);
            (sLeaser.sprites[0] as TriangleMesh).MoveVertice(j * 4 + 1, vector - camPos);
            (sLeaser.sprites[0] as TriangleMesh).MoveVertice(j * 4 + 2, vector - camPos);
            (sLeaser.sprites[0] as TriangleMesh).MoveVertice(j * 4 + 3, vector - camPos);

            (sLeaser.sprites[1] as TriangleMesh).MoveVertice(j * 4, vector - camPos);
            (sLeaser.sprites[1] as TriangleMesh).MoveVertice(j * 4 + 1, vector - camPos);
            (sLeaser.sprites[1] as TriangleMesh).MoveVertice(j * 4 + 2, vector - camPos);
            (sLeaser.sprites[1] as TriangleMesh).MoveVertice(j * 4 + 3, vector - camPos);
        }
    }
}
