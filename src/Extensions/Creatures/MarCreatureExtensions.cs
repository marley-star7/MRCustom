namespace MRCustom.Extensions.Creatures;

public static class MarCreatureExtensions
{
    /// <summary>
    /// Uses source's method of calculating stun from damage, useful for causing a stun same amount as damage without dealing the damage.
    /// </summary>
    /// <param name="damage"></param>
    public static float ConvertDamageToStunBonus(float damage)
    {
        return damage * 30f;
    }

    /// <summary>
    /// Removes the grabbed object from the creature's grasp and the room.
    /// </summary>
    /// <param name="creature"></param>
    /// <param name="graspNum"></param>
    public static void RemoveGrabbedObject(this Creature creature, int graspNum)
    {
        if (creature.grasps[graspNum] == null ||
            creature.grasps[graspNum].grabbed == null)
        {
            return; // Nothing to remove.
        }

        AbstractPhysicalObject abstractPhysicalObject = creature.grasps[graspNum].grabbed.abstractPhysicalObject;
        if (creature.room.game.session is StoryGameSession)
        {
            ((StoryGameSession)creature.room.game.session).RemovePersistentTracker(abstractPhysicalObject);
        }
        creature.ReleaseGrasp(graspNum);
        for (int k = abstractPhysicalObject.stuckObjects.Count - 1; k >= 0; k--)
        {
            if (abstractPhysicalObject.stuckObjects[k] is AbstractPhysicalObject.AbstractSpearStick &&
                abstractPhysicalObject.stuckObjects[k].A.type == AbstractPhysicalObject.AbstractObjectType.Spear &&
                abstractPhysicalObject.stuckObjects[k].A.realizedObject != null)
            {
                ((Spear)abstractPhysicalObject.stuckObjects[k].A.realizedObject).ChangeMode(Weapon.Mode.Free);
            }
        }
        abstractPhysicalObject.LoseAllStuckObjects();
        abstractPhysicalObject.realizedObject.RemoveFromRoom();
        creature.room.abstractRoom.RemoveEntity(abstractPhysicalObject);
    }
}
