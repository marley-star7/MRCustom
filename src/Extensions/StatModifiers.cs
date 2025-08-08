using MRCustom.IdGenerators;

namespace MRCustom.Extensions;

public class StatModifierByteId<ModifierStruct> where ModifierStruct : struct
{
    public ByteIdGenerator IdGenerator = new ByteIdGenerator();
    public readonly Dictionary<byte, ModifierStruct> dictionary = new Dictionary<byte, ModifierStruct>();

    /// <summary>
    /// Add a modifier and return its ID
    /// </summary>
    /// <param name="modifier"></param>
    /// <returns></returns>
    public byte AddModifier(ModifierStruct modifier)
    {
        byte id = IdGenerator.GenerateUniqueId();
        dictionary.Add(id, modifier);
        return id;
    }

    /// <summary>
    /// Remove a modifier by it's id.
    /// </summary>
    /// <param name="modifierId"></param>
    public void RemoveModifier(byte modifierId)
    {
        dictionary.Remove(modifierId);
    }
}
