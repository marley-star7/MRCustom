namespace MRCustom.Physics;

/// <summary>
/// A class for doing the calculations in making easy strings and cords and such visuals.
/// Because I often forget how to copy paste the exact code for what I want lol, instead, we do it here.
/// </summary>
public class MRLinePhysics
{
    public class Part
    {
        public PhysicalObject owner;

        public Vector2 pos;
        public Vector2 lastPos;
        public Vector2 vel;

        public float rad;

        private SharedPhysics.TerrainCollisionData scratchTerrainCollisionData;

        public Part(PhysicalObject owner)
        {
            this.owner = owner;
            pos = owner.firstChunk.pos;
            lastPos = owner.firstChunk.pos;
            vel *= 0f;
        }

        public void Update()
        {
            lastPos = pos;
            pos += vel;
            if (owner.room.PointSubmerged(pos))
            {
                vel *= 0.4f;
                vel.y += 0.1f;
            }
            else
            {
                vel *= 0.88f;
            }

            // The physicsy stuff, to make it interact with the terrain and walls.
            SharedPhysics.TerrainCollisionData collisionData = scratchTerrainCollisionData.Set(pos, lastPos, vel, rad, new IntVector2(0, 0), owner.firstChunk.goThroughFloors);
            collisionData = SharedPhysics.VerticalCollision(owner.room, collisionData);
            collisionData = SharedPhysics.HorizontalCollision(owner.room, collisionData);
            pos = collisionData.pos;
            vel = collisionData.vel;
        }

        public void Reset()
        {
            pos = owner.firstChunk.pos + Custom.RNV() * UnityEngine.Random.value;
            lastPos = pos;
            vel *= 0f;
        }
    }

    public PhysicalObject owner;
    public BodyChunk centerChunk;

    public Part[] parts;
    public int midPart;

    /// <summary>
    /// Collection of parts manually tied to a position.
    /// </summary>
    public Dictionary<int, Vector2> forceSetPartPositions = new();

    /// <summary>
    /// The visual length of a stalk, used to make the stalk longer without a performance hit.
    /// </summary>
    public float partLength = 4f;
    public float restSpeed = 0.5f;

    // TODO: add this functionality
    public float swallowed = 0;

    public MRLinePhysics(PhysicalObject owner, BodyChunk centerChunk, int totalParts)
    {
        this.owner = owner;
        this.centerChunk = centerChunk;

        parts = new MRLinePhysics.Part[totalParts];
        midPart = totalParts / 2;

        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = new MRLinePhysics.Part(owner);
        }
    }

    public void SetPartsRadius(float rad)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].rad = rad;
        }
    }

    public void ResetParts()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].Reset();
        }
    }

    /// <summary>
    ///-MR7: Basically stolen from source, mostly from FlyLure.
    /// </summary>
    /// <param name="i"></param>
    private void ConnectSegment(int i)
    {
        float length = partLength * (1f - swallowed);
        //
        if (i == midPart)
        {
            Vector2 partDirToChunk = Custom.DirVec(parts[i].pos, centerChunk.pos);
            float partDistToChunk = Vector2.Distance(parts[i].pos, centerChunk.pos);
            parts[i].pos -= (length - partDistToChunk) * partDirToChunk * 0.95f;
            parts[i].vel -= (length - partDistToChunk) * partDirToChunk * 0.95f;
            centerChunk.pos += (length - partDistToChunk) * partDirToChunk * 0.05f;
            centerChunk.vel += (length - partDistToChunk) * partDirToChunk * 0.05f;
        }
        if (i > 0)
        {
            Vector2 partDirToPreviousPart = Custom.DirVec(parts[i].pos, parts[i - 1].pos);
            float partDistToPreviousPart = Vector2.Distance(parts[i].pos, parts[i - 1].pos);
            parts[i].pos -= (length - partDistToPreviousPart) * partDirToPreviousPart * 0.5f;
            parts[i].vel -= (length - partDistToPreviousPart) * partDirToPreviousPart * 0.5f;
            parts[i - 1].pos += (length - partDistToPreviousPart) * partDirToPreviousPart * 0.5f;
            parts[i - 1].vel += (length - partDistToPreviousPart) * partDirToPreviousPart * 0.5f;
        }
    }

    public void Update()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].Update();
            parts[i].vel.y -= Mathf.InverseLerp(0f, parts.Length - 1, i) * restSpeed;
        }

        //-- Connect stalks from back to front,
        for (int i = 0; i < parts.Length; i++)
        {
            ConnectSegment(i);
        }

        //-- and then from front to back.
        for (int i = parts.Length - 1; i >= 0; i--)
        {
            ConnectSegment(i);
        }

        for (int i = 0; i < parts.Length; i++)
        {
            //-- MR7 Most of the time, we will be using the direction of the previous PREVIOUS parts,
            // But to make sure the last parts doesn't not settle at all very fast and look weird, we just use the previous.
            Vector2 currentStalkDirToDecidingStalk;
            if (i > 1)
            {
                currentStalkDirToDecidingStalk = Custom.DirVec(parts[i].pos, parts[i - 2].pos);
                parts[i - 2].vel += currentStalkDirToDecidingStalk * (partLength);
            }
            else if (i > 0)
            {
                currentStalkDirToDecidingStalk = Custom.DirVec(parts[i].pos, parts[i - 1].pos);
                parts[i - 1].vel += currentStalkDirToDecidingStalk * (partLength);
            }
            else
            {
                // Cruddy quick fix for the "refuse to settle" issue of the first parts,
                currentStalkDirToDecidingStalk = Custom.DirVec(parts[i].pos, parts[i].pos + Vector2.up * restSpeed);
            }

            //-- MR7: TODO: figure out the magic number 4.5f, in base FlyLure code, it seems to be a multiplier for the stalks.
            // Set it here to be based off parts part length, to be to be similar to the parts length, but not sure if limiting customization?
            parts[i].vel -= currentStalkDirToDecidingStalk * (partLength);
        }

        //-- Once more connect stalks from back to front,
        for (int i = 0; i < parts.Length; i++)
        {
            ConnectSegment(i);
        }
        //-- and back again.
        for (int i = parts.Length - 1; i >= 0; i--)
        {
            ConnectSegment(i);
        }

        //-- Loop through the forced part positions and set them.
        foreach (KeyValuePair<int, Vector2> forcedPartPos in forceSetPartPositions)
        {
            parts[forcedPartPos.Key].pos = forcedPartPos.Value;
            parts[forcedPartPos.Key].vel *= 0;
        }
    }
}
