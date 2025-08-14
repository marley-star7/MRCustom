namespace MRCustom.Physics;

/*
public class MRRopePhysics
{
    public UpdatableAndDeletable owner;

    public Rope rope;

    public MRRopePhysics()
    {

    }

    public void Spawn(Vector2 startPos, Vector2 endPos)
    {
        if (rope != null && rope.visualizer != null)
        {
            rope.visualizer.ClearSprites();
        }
        rope = new Rope(owner.room, startPos, endPos, 1f);
    }

    public void Update(Vector2 startPos, Vector2 endPos)
    {
        rope.Update(startPos, endPos);
    }

    public void Reset(Room newRoom)
    {
        attached = 1f;
        for (int i = 0; i < chunkPoints.GetLength(0); i++)
        {
            chunkPoints[i, 0] = owner.vulture.bodyChunks[4].pos + Custom.RNV();
            chunkPoints[i, 1] = chunkPoints[i, 0];
            chunkPoints[i, 2] *= 0f;
        }
        if (rope != null && rope.visualizer != null)
        {
            rope.visualizer.ClearSprites();
        }
        rope = null;
        for (int j = 0; j < wire.GetLength(0); j++)
        {
            wire[j, 0] = head.pos + Custom.RNV() * UnityEngine.Random.value;
            wire[j, 1] = wire[j, 0];
            wire[j, 2] *= 0f;
            wire[j, 3] *= 0f;
        }
        mode = Mode.Attached;
        modeCounter = 0;
        wireLoose = 0f;
        lastWireLoose = 0f;
        wireExtraSlack = 0f;
        elasticity = 0.9f;
    }

}

*/