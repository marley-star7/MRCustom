namespace MRCustom.Contexts;

public class ViolenceContext
{
    public BodyChunk? source;
    public Vector2? directionAndMomentum;

    public BodyChunk? hitChunk;
    public PhysicalObject.Appendage.Pos? hitAppendage;

    public Creature.DamageType type;
    public float damage;
    public float stunBonus;

    public ViolenceContext(BodyChunk source, Vector2? directionAndMomentum, BodyChunk hitChunk, PhysicalObject.Appendage.Pos hitAppendage, Creature.DamageType type, float damage, float stunBonus)
    {
        this.source = source;
        this.directionAndMomentum = directionAndMomentum;
        this.hitChunk = hitChunk;
        this.hitAppendage = hitAppendage;
        this.type = type;
        this.damage = damage;
        this.stunBonus = stunBonus;
    }
}
