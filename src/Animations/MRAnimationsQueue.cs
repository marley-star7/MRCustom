/*
namespace MRCustom.Animations;

/// <summary>
/// A meta animation queue used to play animations INSIDE of animations based off queue.
/// Waoh...
/// </summary>
public class MRAnimationsQueue<T> : MRAnimation<T> where T : PhysicalObject
{
    public struct AnimationData
    {
        public int currentAnimationIndex = 0;
        public float totalLengthOfAnimationsFinishedSoFar = 0;

        public AnimationData()
        {
        }
    }

    public Dictionary<T, AnimationData> ownerSpecificAnimationData = new();

    public MRAnimation<T>[] animations;

    public MRAnimationsQueue(MRAnimation<T>[] animations)
    {
        this.animations = animations;
        length = 0;
        for (int i = 0; i < animations.Length; i++)
        {
            length += animations[i].length;
        }
    }

    public override void Start(T owner)
    {
        ownerSpecificAnimationData.Add(owner, new AnimationData());
    }

    public void PlayNextAnimationInQueue(T owner)
    {
        var animData = ownerSpecificAnimationData[owner];
        var currentAnimation = animations[animData.currentAnimationIndex];

        currentAnimation.Stop(owner);

        animData.totalLengthOfAnimationsFinishedSoFar += currentAnimation.length;
        animData.currentAnimationIndex++;

        currentAnimation.Start(owner);
    }

    public override void Update(T owner, int animationTimer)
    {
        var animData = ownerSpecificAnimationData[owner];
        var currentAnimation = animations[animData.currentAnimationIndex];

        currentAnimation.Update(owner, animationTimer);

        if (animationTimer >= currentAnimation.length + animData.totalLengthOfAnimationsFinishedSoFar)
            PlayNextAnimationInQueue(owner);
    }

    public override void GraphicsUpdate(T owner, int animationTimer)
    {
        var animData = ownerSpecificAnimationData[owner];
        var currentAnimation = animations[animData.currentAnimationIndex];

        currentAnimation.GraphicsUpdate(owner, animationTimer);
    }

    public override void Stop(T owner)
    {
        var animData = ownerSpecificAnimationData[owner];
        var currentAnimation = animations[animData.currentAnimationIndex];

        ownerSpecificAnimationData.Remove(owner);
    }
}
*/