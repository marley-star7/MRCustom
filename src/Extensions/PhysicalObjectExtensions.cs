namespace MRCustom.Extensions;

// This class and whole functionality stuff is to make setting rotation of various objects much easier.
// Normal game re-sets up rotation values in multiple objects in multiple different ways,
// So "if-statementing" all of them looking for what object it is JUST to set the rotation whenever i want to do it I decided for this instead.

public class PhysicalObjectData
{
    /// <summary>
    /// A dictionary is used to take advantage of hashing mainly, intuitive to work with.
    /// </summary>
    public Dictionary<Type, RWModule> Modules = new();

    public WeakReference<PhysicalObject> physicalObjectRef;

    public PhysicalObjectData(PhysicalObject physicalObject)
    {
        physicalObjectRef = new WeakReference<PhysicalObject>(physicalObject);
    }

    public RWModule GetModule(Type moduleType)
    {
        return Modules[moduleType];
    }

    public T GetModule<T>() where T : RWModule
    {
        return (T)Modules[typeof(T)];
    }

    public bool TryGetModule<T>(out T module) where T : RWModule
    {
        bool result = Modules.TryGetValue(typeof(T), out var moduleValue);
        if (result)
        {
            module = (T)moduleValue;
        }
        else
        {
            module = null;
        }
        return result;
    }

    public void AddModule(RWModule module)
    {
        Modules.Add(module.moduleType, module);
    }
}

public static class PhysicalObjectExtensions
{
    internal static readonly ConditionalWeakTable<PhysicalObject, PhysicalObjectData> dataConditionalWeakTable = new();

    //-- Module Helper Functions
    public static RWModule GetModule(this PhysicalObject physicalObject, Type moduleType)
    {
        return physicalObject.GetPhysicalObjectMarData().Modules[moduleType];
    }

    public static T GetModule<T>(this PhysicalObject physicalObject) where T : RWModule
    {
        return physicalObject.GetPhysicalObjectMarData().GetModule<T>();
    }

    public static bool TryGetModule<T>(this PhysicalObject physicalObject, out T module) where T : RWModule
    {
        return physicalObject.GetPhysicalObjectMarData().TryGetModule(out module);
    }

    internal static PhysicalObjectData GetPhysicalObjectMarData(this PhysicalObject physicalObject) => dataConditionalWeakTable.GetValue(physicalObject, _ =>
    {
        return new PhysicalObjectData(physicalObject);
    });

    public static void AddModule(this PhysicalObject physicalObject, RWModule module)
    {
        physicalObject.GetPhysicalObjectMarData().AddModule(module);
    }

    public static int GetClosestBodyChunkIndex(this PhysicalObject physicalObject, Vector2 pos)
    {
        int closestBodyChunkIndex = -1;
        float closestDistanceSoFar = 9999999f;

        for (int i = 0; i < physicalObject.bodyChunks.Length; i++)
        {
            var distance = Custom.Dist(physicalObject.bodyChunks[i].pos, pos);
            // If we are not inside chunk rad, add chunk rads to the check, since it means we are outside teh chunk.
            if (distance > physicalObject.bodyChunks[i].rad)
            {
                distance += physicalObject.bodyChunks[i].rad;
            }

            if (distance < closestDistanceSoFar)
            {
                closestDistanceSoFar = distance;
                closestBodyChunkIndex = i;
            }
        }

        return closestBodyChunkIndex;
    }

    public static BodyChunk GetClosestBodyChunk(this PhysicalObject physicalObject, Vector2 pos)
    {
        var closestBodyChunkIndex = physicalObject.GetClosestBodyChunkIndex(pos);

        if (closestBodyChunkIndex == -1)
        {
            return null;
        }

        return physicalObject.bodyChunks[closestBodyChunkIndex];
    }

    /// <summary>
    /// Gets the closest body chunk in a range,
    /// Range check does not check for bodyChunks the pos is inside.
    /// </summary>
    /// <param name="physicalObject"></param>
    /// <param name="pos"></param>
    /// <param name="rangeRad"></param>
    /// <returns></returns>
    public static int GetClosestBodyChunkIndexInRange(this PhysicalObject physicalObject, Vector2 pos, float rangeRad)
    {
        int closestBodyChunkIndex = -1;
        float closestDistanceSoFar = 99999999f;

        for (int i = 0; i < physicalObject.bodyChunks.Length; i++)
        {
            var distance = Custom.Dist(physicalObject.bodyChunks[i].pos, pos);

            // If we are not inside chunk rad, add chunk rads to the check, since it means we are outside teh chunk.
            if (distance > physicalObject.bodyChunks[i].rad)
            {
                distance += physicalObject.bodyChunks[i].rad;

                // Have to do range check then
                if (distance > rangeRad)
                {
                    continue;
                }
            }

            if (distance < closestDistanceSoFar)
            {
                closestDistanceSoFar = distance;
                closestBodyChunkIndex = i;
            }
        }

        return closestBodyChunkIndex;
    }

    public static BodyChunk GetClosestBodyChunkInRange(this PhysicalObject physicalObject, Vector2 pos, float rangeRad)
    {
        var closestBodyChunkIndex = physicalObject.GetClosestBodyChunkIndexInRange(pos, rangeRad);

        if (closestBodyChunkIndex == -1)
        {
            return null;
        }

        return physicalObject.bodyChunks[closestBodyChunkIndex];
    }
}