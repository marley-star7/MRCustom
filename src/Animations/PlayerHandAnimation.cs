namespace MRCustom.Animations;

public abstract class PlayerHandAnimation
{
    /// <summary>
    /// How long the animation lasts.
    /// </summary>
    public float Length;

    public PlayerHandAnimation(float length)
    {
        Length = length;
    }

    /// <summary>
    /// Runs when the animation is started for the first time.
    /// </summary>
    /// <param name="creature"></param>
    public abstract void Play(Player player);

    /// <summary>
    /// Runs on player update.
    /// </summary>
    /// <param name="player"></param>
    public abstract void Update(Player player);

    /// <summary>
    /// Runs on player graphics update.
    /// </summary>
    /// <param name="playerGraphics"></param>
    public abstract void GraphicsUpdate(PlayerGraphics playerGraphics);
}
