namespace MRCustom.Extensions;

public static class SlugcatStatsExtensions
{
    private static Dictionary<SlugcatStats.Name, bool> _slugcatAbleToPickupStunnedLargeCreatures = new();

    public static void SetSlugcatCanPickupStunnedLargeCreatures(SlugcatStats.Name slugcatClass, bool value)
    {
        _slugcatAbleToPickupStunnedLargeCreatures[slugcatClass] = value;
    }

    public static bool SlugcatCanPickupStunnedLargeCreatures(SlugcatStats.Name slugcatClass)
    {
        if (SlugcatStats.SlugcatCanMaul(slugcatClass))
        {
            return true;
        }

        if (_slugcatAbleToPickupStunnedLargeCreatures.TryGetValue(slugcatClass, out var result))
        {
            return result;
        }

        return false;
    }
}
