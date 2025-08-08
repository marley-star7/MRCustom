using ImprovedInput;

namespace MRCustom.Extensions;

public static class CustomInputExtExtensions
{
    public static bool JustReleased(this Player player, PlayerKeybind key)
    {
        CustomInput[] inputHistory = player.InputHistory();
        return !inputHistory[0][key] && inputHistory[1][key];
    }
}
