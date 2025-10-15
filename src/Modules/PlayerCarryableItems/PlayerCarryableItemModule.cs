namespace MRCustom.Modules.PlayerCarryableItems;

public class PlayerCarryableItemModule : PhysicalObjectModule
{
    public new PlayerCarryableItem Owner => (PlayerCarryableItem)base._owner;

    public PlayerCarryableItemModule(PlayerCarryableItem owner, Type moduleType) : base(owner, moduleType)
    {

    }
}
