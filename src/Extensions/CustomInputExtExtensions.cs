using ImprovedInput;

namespace MRCustom.Extensions;

public static class CustomInputExtExtensions
{
    /// <summary>
    /// Returns true the frame an input has been released.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool JustReleased(this Player player, PlayerKeybind key)
    {
        CustomInput[] inputHistory = player.InputHistory();
        return !inputHistory[0][key] && inputHistory[1][key];
    }
}
