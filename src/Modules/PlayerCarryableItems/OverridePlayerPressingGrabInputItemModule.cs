namespace MRCustom.Modules.PlayerCarryableItems;

// Actual functionality added to item for unique way it overrides the input.
public class OverridePlayerPressingGrabInputItemModule : PlayerCarryableItemModule
{
    public IOverridePlayerPressingGrabInput inputActionItemModule;

    public OverridePlayerPressingGrabInputItemModule(PlayerCarryableItem owner, IOverridePlayerPressingGrabInput inputActionItemModule) : base(owner, typeof(OverridePlayerPressingGrabInputItemModule))
    {
        this.inputActionItemModule = inputActionItemModule;
    }

    public virtual void PlayerJustPressedGrabUpdate(Player player)
    {
        inputActionItemModule.PlayerJustPressedGrabUpdate(player);
    }

    public virtual void PlayerPressingGrabUpdate(Player player)
    {
        inputActionItemModule.PlayerPressingGrabUpdate(player);
    }

    public virtual void PlayerJustReleasedGrabUpdate(Player player)
    {
        inputActionItemModule.PlayerJustReleasedGrabUpdate(player);
    }

    public virtual void PlayerNotPressingGrabUpdate(Player player)
    {
        inputActionItemModule.PlayerNotPressingGrabUpdate(player);
    }
}

//-- Ms7: Interface is just so you can check if a module is one of these types.
public interface IOverridePlayerPressingGrabInput
{
    public OverridePlayerPressingGrabInputItemModule OverridePlayerPressingGrabInputModule { get; }

    public void PlayerJustPressedGrabUpdate(Player player);

    public void PlayerPressingGrabUpdate(Player player);

    public void PlayerJustReleasedGrabUpdate(Player player);

    public void PlayerNotPressingGrabUpdate(Player player);
}
