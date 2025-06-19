namespace MRCustom.Animations;

public class MRAnimationPlayer<T> where T : PhysicalObject
{
    public MRAnimation<T>? currentAnimation = null;

    public int animationTimer = 0;
    public bool isPlaying = false;

    public WeakReference<T> ownerRef;

    public MRAnimationPlayer(T owner)
    {
        this.ownerRef = new WeakReference<T>(owner);
    }

    // Just for ease of use refrencing.
    public T owner{
        get {
            if (ownerRef.TryGetTarget(out T owner))
                return owner;
            else
                return null;
        }
    }

    /// <summary>
    /// Play the animation if paused,
    /// Start it for the first time if it is a new animation.
    /// </summary>
    /// <param name="animation"></param>
    public void Play(MRAnimation<T> animation)
    {
        isPlaying = true;

        // If this is a new animation, start it from the beginning.
        if (currentAnimation != animation)
            Start(animation);
    }

    private void Start(MRAnimation<T> animation)
    {
        animationTimer = 0;
        currentAnimation = animation;

        currentAnimation.Start(owner);
    }

    public void Pause()
    {
        isPlaying = false;
    }

    /// <summary>
    /// Stop only if current animation is same as requested to stop.
    /// </summary>
    /// <param name="animation"></param>
    public void Stop(MRAnimation<T> animation)
    {
        if (currentAnimation == animation)
        {
            currentAnimation.Stop(owner);
            currentAnimation = null;

            isPlaying = false;
            animationTimer = 0;
        }
    }


    public void Stop()
    {
        currentAnimation.Stop(owner);
        currentAnimation = null;

        isPlaying = false;
        animationTimer = 0;
    }

    public Action<MRAnimation<T>?> AnimationFinished;

    public void Update()
    {
        if (!isPlaying || currentAnimation == null)
            return;

        animationTimer++;

        currentAnimation.Update(animationTimer);

        if (animationTimer > currentAnimation.Length)
            FinishAnimation();
    }

    // TODO: find out how to do delta junk or whatever for the proper animationTimer++ timing when updating, idk what rain world uses.
    public void GraphicsUpdate()
    {
        if (!isPlaying || currentAnimation == null)
            return;

        currentAnimation.GraphicsUpdate(animationTimer);

        // I do this in both because, playing is safe :[ 
        if (animationTimer > currentAnimation.Length)
            FinishAnimation();
    }

    private void FinishAnimation()
    {
        var finishedAnim = currentAnimation;
        Stop();
        AnimationFinished?.Invoke(finishedAnim);
    }
}