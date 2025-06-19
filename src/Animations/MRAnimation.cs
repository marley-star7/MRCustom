namespace MRCustom.Animations;

public abstract class MRAnimation<T> where T : PhysicalObject
{
    /*
    public enum LoopModeEnum
    {
        None,
        Linear,
        PingPong
    }
    public LoopModeEnum LoopMode;
    */

    protected T owner;
    public float Length;

    /// <summary>
    /// Start the animation.
    /// </summary>
    /// <param name="owner"></param>
    public virtual void Start(T owner)
    {
        this.owner = owner;
    }
    /// <summary>
    /// Stops the animation.
    /// </summary>
    /// <param name="owner"></param>
    public abstract void Stop(T owner);
    /// <summary>
    /// Ran on normal update.
    /// </summary>
    /// <param name="animationTime"></param>
    public abstract void Update(int animationTimer);
    /// <summary>
    /// Ran on graphics update.
    /// </summary>
    /// <param name="animationTime"></param>
    public abstract void GraphicsUpdate(int animationTimer);
}