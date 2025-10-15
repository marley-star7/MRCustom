namespace MRCustom.Modules.Creatures;

public class CreatureModule : PhysicalObjectModule
{
    public new Creature Owner => (Creature)base._owner;

    public CreatureModule(Creature owner, Type moduleType) : base(owner, moduleType)
    {

    }
}
