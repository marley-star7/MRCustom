/*
// This whole thing is stupid, and overcomplicated probably,
// but I couldn't think of a better way to do it without just making all hand animations a big line of if statements.

namespace MRCustom.Animations;

public sealed class MRAnimationPlayer<T> where T : PhysicalObject
{
    public MRAnimationPlayerIndex<T>? index = null;
    public MRAnimation<T>? currentAnimation = null;

    public int animationTimer = 0;
    public bool isPlaying = false;

    public WeakReference<T> ownerRef;

    public MRAnimationPlayer(T owner, MRAnimationPlayerIndex<T>? index)
    {
        this.ownerRef = new WeakReference<T>(owner);
        this.index = index;
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

    public void Play(string animationName)
    {
        if (index.Animations.TryGetValue(animationName, out var animation))
            Play(animation);
    }

    private void Play(MRAnimation<T> animation)
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

    public void Stop()
    {
        isPlaying = false;
        animationTimer = 0;

        currentAnimation.Stop(owner);
        currentAnimation = null;
    }

    public Action<string> AnimationFinished;

    public void Update()
    {
        if (!isPlaying || currentAnimation == null)
            return;

        animationTimer++;

        if (animationTimer > currentAnimation.GetLength(owner))
        {
            var finishedAnim = currentAnimation;
            Stop();
            AnimationFinished?.Invoke(finishedAnim.Name);
        }

        currentAnimation.Update(owner, animationTimer);
    }

    public void GraphicsUpdate()
    {
        if (!isPlaying || currentAnimation == null)
            return;

        currentAnimation.UpdateGraphics(owner, animationTimer);
    }
}
*/