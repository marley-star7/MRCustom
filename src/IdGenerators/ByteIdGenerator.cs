namespace MRCustom.IdGenerators;

/// <summary>
/// A Byte id generator, can store up to 254 values.
/// Id 0 is reserved for unassaigned id's, and is therefore never used.
/// </summary>
public class ByteIdGenerator
{
    private byte _nextId; // Keeps track of the next unique ID to be assigned
    private HashSet<byte> _usedIds; // Tracks IDs currently in use
    private Queue<byte> _recycledIds; // Recycled IDs available for reuse

    public ByteIdGenerator()
    {
        _nextId = 1; // Start from 1 (0 reserved for invalid/empty case)
        _usedIds = new HashSet<byte>();
        _recycledIds = new Queue<byte>();
    }

    public byte GenerateUniqueId()
    {
        if (_usedIds.Count >= byte.MaxValue - 1) // -1 because we reserve 0
            throw new Exception("Maximum number of active byte IDs reached");

        byte id;
        if (_recycledIds.Count > 0)
        {
            // Reuse a recycled ID
            id = _recycledIds.Dequeue();
        }
        else
        {
            // Generate new ID (with wrap-around check)
            if (_nextId == 0) _nextId = 1; // Skip 0 (reserved)
            id = _nextId++;
        }

        _usedIds.Add(id);
        return id;
    }

    public void ReleaseId(byte id)
    {
        if (id == 0) return; // Never release the reserved 0 ID

        if (_usedIds.Contains(id))
        {
            _usedIds.Remove(id);
            _recycledIds.Enqueue(id);
        }
    }

    // Optional helper method to check if an ID is currently in use
    public bool IsIdInUse(byte id) => _usedIds.Contains(id);

    // Optional property to track remaining available IDs
    public int AvailableIds => byte.MaxValue - 1 - _usedIds.Count; // -1 for reserved 0
}