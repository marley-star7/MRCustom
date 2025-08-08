namespace MRCustom.IdGenerators;

public class UShortIdGenerator
{
    private ushort _nextId; // Keeps track of the next unique ID to be assigned
    private HashSet<ushort> _usedIds; // Keeps track of IDs that have already been used
    private Queue<ushort> _recycledIds; // A queue of recycled IDs that can be reused

    public UShortIdGenerator()
    {
        _nextId = 1; // Start ID from 1, 0 is saved if ever need invalid Id use case scenario.
        _usedIds = new HashSet<ushort>();
        _recycledIds = new Queue<ushort>();
    }

    public ushort GenerateUniqueId()
    {
        ushort id;
        if (_recycledIds.Count > 0)
        {
            // Reuse an ID from the pool of recycled IDs
            id = _recycledIds.Dequeue();
        }
        else
        {
            // Generate a new ID
            id = _nextId++;
        }

        _usedIds.Add(id);
        return id;
    }

    public void ReleaseId(ushort id)
    {
        // Mark this ID as released and add it back to the recycled pool
        if (_usedIds.Contains(id))
        {
            _usedIds.Remove(id);
            _recycledIds.Enqueue(id);
        }
    }
}