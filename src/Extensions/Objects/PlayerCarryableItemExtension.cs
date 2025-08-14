/*
namespace MRCustom.Extensions.Objects;

public class PlayerCarryableItemMarData
{
    public WeakReference<PlayerCarryableItem> playerCarryableItemRef;

    public PlayerCarryableItemMarData(PlayerCarryableItem playerCarryableItem)
    {
        playerCarryableItemRef = new WeakReference<PlayerCarryableItem>(playerCarryableItem);
    }

    public virtual void SetItemRotation(Vector2 rotation)
    {

    }
}

public static class PlayerCarryableItemMarExtensions
{
    private static readonly ConditionalWeakTable<PlayerCarryableItem, PlayerCarryableItemMarData> craftingDataConditionalWeakTable = new();

    public static PlayerCarryableItemMarData GetPlayerCarryableItemMarData(this PlayerCarryableItem playerCarryableItem) => craftingDataConditionalWeakTable.GetValue(playerCarryableItem, _ =>
    {
        // Check type at creation time
        if (playerCarryableItem is Spear spear)
            return new SpearMarData(spear);
        else
            return new PlayerCarryableItemMarData(playerCarryableItem);
    });
    public static void SetItemRotation(this PlayerCarryableItem playerCarryableItem, Vector2 rotation)
    {
        playerCarryableItem.GetPlayerCarryableItemMarData().SetItemRotation(rotation);
    }
}
*/