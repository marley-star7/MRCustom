namespace MRCustom;

/// <summary>
/// Represents the definition of an object, including its type and optional creature type if it is a creature.
/// </summary>
/// <remarks>This structure is used to define an object, such as a physical object or a creature, by
/// specifying its  <see cref="AbstractPhysicalObject.AbstractObjectType"/> and, if applicable, its <see
/// cref="CreatureTemplate.Type"/>.</remarks>
public readonly struct EntityTypeDefinition
{
    /// <summary>
    /// The AbstractObjectType of the ingredient.
    /// Set as AbstractObjectType.Creature if you wish to make the ingredient for a creature.
    /// </summary>
    public readonly AbstractPhysicalObject.AbstractObjectType? objectType;
    /// <summary>
    /// If this craft ingredient is a creature, the type of that creature.
    /// </summary>
    public readonly CreatureTemplate.Type? creatureType;

    public EntityTypeDefinition(AbstractPhysicalObject.AbstractObjectType objectType, CreatureTemplate.Type? creatureType = null)
    {
        this.objectType = objectType;
        this.creatureType = creatureType;
    }

    /// <summary>
    /// Automatically creates an EntityTypeDefinition for an AbstractPhysicalObject's information.
    /// </summary>
    /// <param name="abstractPhysicalObject"></param>
    public EntityTypeDefinition(AbstractPhysicalObject abstractPhysicalObject)
    {
        this.objectType = abstractPhysicalObject.type;
        if (abstractPhysicalObject is AbstractCreature abstractCreature)
            this.creatureType = abstractCreature.creatureTemplate.type;
        else
            this.creatureType = null;
    }

    //-- Ms7: Operator overloading for increased performance when comparing EntityTypeDefinitions,
    // since we do not need to check if creatureType is the same when objectType is not Creature.

    /// <summary>
    /// Compares two EntityTypeDefinition instances for equality.
    /// </summary>
    /// <remarks>
    /// If both object types are Creature, compares creature types as well.
    /// For non-creature object types, only the object type is compared.
    /// </remarks>
    public static bool operator ==(EntityTypeDefinition left, EntityTypeDefinition right)
    {
        // If object types don't match, they're not equal
        if (left.objectType != right.objectType)
            return false;

        // If both are creatures, compare creature types
        if (left.objectType == AbstractPhysicalObject.AbstractObjectType.Creature)
        {
            return left.creatureType == right.creatureType;
        }

        // For non-creature types, object type match is sufficient
        return true;
    }

    /// <summary>
    /// Compares two EntityTypeDefinition instances for inequality.
    /// </summary>
    public static bool operator !=(EntityTypeDefinition left, EntityTypeDefinition right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current EntityTypeDefinition.
    /// </summary>
    public override bool Equals(object obj)
    {
        return obj is EntityTypeDefinition other && this == other;
    }

    /// <summary>
    /// Returns the hash code for this EntityTypeDefinition.
    /// </summary>
    public override int GetHashCode()
    {
        if (objectType == AbstractPhysicalObject.AbstractObjectType.Creature)
        {
            return (objectType, creatureType).GetHashCode();
        }
        else
        {
            // For non-creatures, only use objectType
            return objectType.GetHashCode();
        }
    }
}