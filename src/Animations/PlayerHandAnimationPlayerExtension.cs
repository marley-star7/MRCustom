using RWCustom;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace MRCustom.Animations;

public class PlayerHandAnimationData
{
    public partial class HandAnimationIndex : ExtEnum<HandAnimationIndex>
    {
        public static readonly HandAnimationIndex None = new HandAnimationIndex("None", register: true);

        public HandAnimationIndex(string value, bool register = false)
            : base(value, register)
        {
        }
    }

    public HandAnimationIndex handAnimationIndex;
    public PlayerHandAnimation handAnimation;

    public void PlayHandAnimation(PlayerHandAnimation newHandAnimation)
    {
        if (handAnimation != newHandAnimation)
        {
            handAnimation = newHandAnimation;
            if (playerRef.TryGetTarget(out Player player))
            {
                handAnimation.Play(player);
            }
        }
    }

    public WeakReference<Player> playerRef;

    public PlayerHandAnimationData(Player player)
    {
        this.playerRef = new WeakReference<Player>(player);
    }
}

/// <summary>
/// Extends the Player class to add hand animation functionality.
/// </summary>
public static class PlayerHandAnimationPlayerExtension
{
    private static readonly ConditionalWeakTable<Player, PlayerHandAnimationData> handAnimationDataConditionalWeakTable = new();

    public static PlayerHandAnimationData GetHandAnimationData(this Player player) => handAnimationDataConditionalWeakTable.GetValue(player, _ => new PlayerHandAnimationData(player));

    //
    // OLD OVERENGINEERED ANIMATION SYSTEM
    //

    /*
    // TODO: later somehow probably set this up to work with changing the game's actual animation index, alongside this system.
    // TODO: ideally some getter / setter overriding when the animation changes, and automatically playing the new one.

    private static MRAnimationPlayerIndex<Player> index = new();

    /// <summary>
    /// Registers a new hand animation to the player animation index.
    /// </summary>
    /// <param name="animation"></param>
    public static string RegisterHandAnimation(MRAnimation<Player> animation)
    {
        index.Register(animation);
        return animation.Name;
    }

    //
    // WEAK REFRENCE WE ALWAYS HAVE FOR THESE STUPID THINGS
    //

    private static readonly ConditionalWeakTable<Player, MRAnimationPlayer<Player>> handAnimationDataConditionalWeakTable = new();

    /// <summary>
    /// Gets the hand animation player for the player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static MRAnimationPlayer<Player> GetHandAnimationData(this Player player) => handAnimationDataConditionalWeakTable.GetValue(player, _ => new MRAnimationPlayer<Player>(player, index));
    */
}