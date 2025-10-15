/*
namespace MRCustom.Extensions.Objects;

public class SpearMarData : PlayerCarryableItemMarData
{
    public WeakReference<Spear> spearRef;

    public SpearMarData(Spear spear) : base(spear)
    {
        spearRef = new WeakReference<Spear>(spear);
    }

    public override void SetItemRotation(Vector2 rotation)
    {
        if (spearRef.TryGetTarget(out var spear));
            spear.setRotation = rotation;
    }
}
*/

/*
public static class SpearExtensions
{
    public static void Deflect(this Spear spear)
    {
        // Spear bounces off.
        spear.vibrate = 20;
        spear.ChangeMode(Weapon.Mode.Free);
        spear.firstChunk.vel = spear.firstChunk.vel * -0.5f + Custom.DegToVec(Random.value * 360f) * Mathf.Lerp(0.1f, 0.4f, Random.value) * spear.firstChunk.vel.magnitude;
        spear.SetRandomSpin();
    }
}
*/