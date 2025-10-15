namespace MRCustom.Modules.Creatures;

public class PlayerModule : CreatureModule
{
    public new Player Owner => (Player)base._owner;

    public PlayerModule(Player owner, Type moduleType) : base(owner, moduleType)
    {

    }
}
