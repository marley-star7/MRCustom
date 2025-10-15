namespace MRCustom.Extensions;

public static class PoleMimicExtensions
{
    public static void Sever(this PoleMimic poleMimic, PhysicalObject.Appendage.Pos hitAppendage)
    {
        var abstractData = poleMimic.abstractCreature.GetMarAbstractPoleMimicData();

        if (hitAppendage == null)
        {
            return;
        }

        int prevPos = abstractData.cutAppendage != -1 ? abstractData.cutAppendage : hitAppendage.appendage.segments.Length;

        abstractData.cutAppendage = Mathf.Min(hitAppendage.prevSegment, prevPos);

        // TODO: You should probabily figure out how to change the hitbox or hook a few tings so nothing can hit the pole mimic as I just basically only moved the sprites
        poleMimic.Die();
    }
}
