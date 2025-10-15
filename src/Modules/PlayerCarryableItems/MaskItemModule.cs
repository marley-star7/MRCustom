namespace MRCustom.Modules.PlayerCarryableItems;

public class DonnableMaskItemModule : PlayerCarryableItemModule
{
    // Value from 0 to 1
    public float donned;

    public Vector2 donnedHandAbsoluteHuntPos = Vector2.zero;
    public Vector2 donnedHandRelativeHuntPos = Vector2.zero;

    public SlugcatHand.Mode slugcatHandMode = SlugcatHand.Mode.HuntAbsolutePosition;

    public DonnableMaskItemModule(PlayerCarryableItem owner) : base(owner, typeof(DonnableMaskItemModule))
    {
    }
}
