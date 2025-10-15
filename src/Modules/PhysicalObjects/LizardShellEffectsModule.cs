using Fisobs.Properties;
using Noise;

namespace MRCustom.Modules.PhysicalObjects;

public class LizardShellEffectsModule : RWModule
{
    public LizardEffectColorGraphics effectColorGraphics;

    public float terrainImpactNoiseModifier = 3;

    public Color RawColor
    {
        get => effectColorGraphics.rawColor;
        set => effectColorGraphics.rawColor = value;
    }

    public LizardShellEffectsModule(UpdatableAndDeletable Owner, Color color) : base(Owner, typeof(LizardShellEffectsModule))
    {
        effectColorGraphics = new LizardEffectColorGraphics(color);
    }

    public void Update()
    {
        effectColorGraphics.Update();
    }

    public void DrawSpritesUpdate()
    {
        effectColorGraphics.DrawSpritesUpdate();
    }

    public void WhiteFlicker()
    {
        effectColorGraphics.WhiteFlicker();
    }

    public void WhiteFlicker(int fl)
    {
        effectColorGraphics.WhiteFlicker(fl);
    }

    public void Flicker()
    {
        effectColorGraphics.Flicker();
    }

    public void Flicker(int fl)
    {
        effectColorGraphics.Flicker(fl);
    }

    private static float Rand => Random.value;

    private void SpawnSparks(UpdatableAndDeletable Owner, Vector2 sourcePos, Vector2 directionAndMomentum, int sparkNum)
    {
        Color sparkColor = effectColorGraphics.ShellColor();

        for (int k = 0; k < sparkNum; k++)
        {
            Vector2 pos = sourcePos + Custom.DegToVec(Rand * 360f) * 5f * Rand;
            Vector2 vel = -directionAndMomentum * -0.1f + Custom.DegToVec(Rand * 360f) * Mathf.Lerp(0.2f, 0.4f, Rand) * directionAndMomentum.magnitude;

            var spark = new Spark(pos, vel, sparkColor, null, 10, 170);
            spark.GetMarSparkData().lizardShellColorGraphics = effectColorGraphics; // Set color effect graphics so can have same effect as lizords yay.

            Owner.room.AddObject(spark);
        }
    }

    public void DoTerrainImpactEffects(BodyChunk impactChunk, Vector2 direction, float speed, bool firstContact)
    {
        var Owner = impactChunk.owner;
        var vol = Mathf.Clamp(speed * 0.07f, 0, 0.7f); //--MS7: Limit volume to not blow your ears off lol.
        var noiseStrength = Mathf.Clamp(speed * terrainImpactNoiseModifier, 0, 100f);

        Owner.room.PlaySound(SoundID.Spear_Fragment_Bounce, impactChunk, false, vol, Random.Range(0.8f, 1.2f));
        Owner.room.InGameNoise(new InGameNoise(impactChunk.pos, noiseStrength, Owner, 1f));
        int sparkNum = (int)Random.Range(vol * 2, vol * 7);

        Flicker((int)speed);
        //lizardEffectColorGraphics.Flicker((int)Mathf.Max(speed * 0.3f, 30));
        SpawnSparks(Owner, impactChunk.pos, direction * speed, sparkNum);
    }

    /// <summary>
    /// Visual and audio queues for deflecting a hit.
    /// </summary>
    /// <param name="chunkHit"></param>
    /// <param name="sourcePos"></param>
    /// <param name="directionAndMomentum"></param>
    /// <param name="flickerTime"></param>
    public void DoDeflectEffects(BodyChunk chunkHit, Vector2 sourcePos, Vector2 directionAndMomentum, int flickerTime)
    {
        effectColorGraphics.WhiteFlicker(flickerTime);

        var orb = new StationaryEffect(
            sourcePos,
            new Color(1f, 1f, 1f),
            null,
            StationaryEffect.EffectType.FlashingOrb);

        orb.GetMarStationaryEffectData().lizardShellColorGraphics = effectColorGraphics; // Set color effect graphics so can have same effect as lizords yay.
        chunkHit.owner.room.AddObject(orb);

        SpawnSparks(chunkHit.owner, sourcePos, directionAndMomentum, Random.Range(3, 8));

        chunkHit.owner.room.PlaySound(SoundID.Spear_Bounce_Off_Creauture_Shell, chunkHit);
    }

    public void DoDeflectEffects(BodyChunk chunkHit, Vector2 sourcePos, Vector2 directionAndMomentum, float damage, float stunBonus)
    {
        float flickerTimeF = (damage * 30f + stunBonus);
        int flickerTime = (int)(Mathf.Clamp(flickerTimeF, 25f, damage * 30f));
        DoDeflectEffects(chunkHit, sourcePos, directionAndMomentum, flickerTime);
    }

    public void DoShatterEffects(Vector2 pos)
    {
        Owner.room.PlaySound(SoundID.Spear_Fragment_Bounce, pos, 0.35f, 2f);

        for (int k = 0; k < 5; k++)
        {
            Owner.room.AddObject(new LizardShellFragment(
                pos, 
                Custom.RNV() * Mathf.Lerp(5f, 15f, Random.value), 
                effectColorGraphics.ShellColor()));
        }
    }
}
