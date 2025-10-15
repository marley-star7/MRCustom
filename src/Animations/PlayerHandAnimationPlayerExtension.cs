using static MRCustom.Animations.PlayerHandAnimationPlayer;

namespace MRCustom.Animations;

public class PlayerHandAnimationPlayer : RWAnimationPlayer<AnimationIndex, Player>
{
    public class AnimationIndex : ExtEnum<AnimationIndex>
    {
        public static readonly AnimationIndex None = new AnimationIndex("None", register: true);

        public AnimationIndex(string name, bool register)
            : base(name, register) { }
    }

    public static AnimationLibrary<AnimationIndex, Player> defaultPlayerHandAnimationLibrary = new();

    public WeakReference<Player> playerRef;

    public PlayerHandAnimationPlayer(Player player) : base(defaultPlayerHandAnimationLibrary, player)
    {
        this.playerRef = new WeakReference<Player>(player);
    }
}

/// <summary>
/// Extends the Player class to add hand animation functionality.
/// </summary>
public static class PlayerHandAnimationPlayerExtension
{
    private static readonly ConditionalWeakTable<Player, PlayerHandAnimationPlayer> handAnimationDataConditionalWeakTable = new();

    public static PlayerHandAnimationPlayer GetHandAnimationPlayer(this Player player) => handAnimationDataConditionalWeakTable.GetValue(player, _ => new PlayerHandAnimationPlayer(player));
}