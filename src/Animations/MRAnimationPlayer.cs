namespace MRCustom.Animations;

/// <summary>
/// Handles playback of animations for a PhysicalObject owner, managing animation states,
/// events, and transitions. Works with an AnimationLibrary for animation storage.
/// </summary>
/// <typeparam name="AnimationLibraryType">ExtEnum type used as animation identifiers</typeparam>
/// <typeparam name="ownerType">Type of the object owning these animations</typeparam>
public class MRAnimationPlayer<AnimationLibraryType, ownerType>
    where AnimationLibraryType : ExtEnum<AnimationLibraryType>
    where ownerType : PhysicalObject
{
    // --- Fields ---
    private AnimationLibraryType? _currentAnimationIndex;
    private MRAnimation<ownerType>? _currentAnimation;
    private int _waitingForAnimationEventIndex;
    private float _animationTimer;
    private int _timesLoopedCurrentAnimation;
    private bool _isPlaying;

    // --- Properties ---
    public readonly AnimationLibrary<AnimationLibraryType, ownerType> animationLibrary;
    public WeakReference<ownerType> ownerRef { get; private set; }

    public float animationTimer
    {
        get => _animationTimer;
    }

    public int timesLoopedCurrentAnimation
    {
        get => _timesLoopedCurrentAnimation;
    }

    public bool isPlaying
    {
        get => _isPlaying;
    }

    /// <summary>
    /// The currently playing animation's index in the library
    /// </summary>
    public AnimationLibraryType CurrentAnimationIndex => _currentAnimationIndex;

    /// <summary>
    /// Reference to the currently playing animation
    /// </summary>
    public MRAnimation<ownerType> CurrentAnimation => _currentAnimation;

    /// <summary>
    /// The owner object of this animation player
    /// </summary>
    public ownerType owner => ownerRef.TryGetTarget(out ownerType owner) ? owner : null;

    /// <summary>
    /// Whether any animation is currently playing
    /// </summary>
    public bool IsPlaying => _isPlaying;

    // --- Events ---
    /// <summary>
    /// Called when an animation completes a loop
    /// </summary>
    public event Action<AnimationLibraryType, int> AnimationLooped;

    /// <summary>
    /// Called when an animation finishes completely (won't fire for looping animations)
    /// </summary>
    public event Action<AnimationLibraryType, int> AnimationFinished;

    // --- Constructor ---
    public MRAnimationPlayer(AnimationLibrary<AnimationLibraryType, ownerType> animationLibrary, ownerType owner)
    {
        this.animationLibrary = animationLibrary;
        this.ownerRef = new WeakReference<ownerType>(owner);
    }

    // --- Public Methods ---
    /// <summary>
    /// Starts or resumes playback of an animation by its index
    /// </summary>
    public void Play(AnimationLibraryType animationIndex)
    {
        _isPlaying = true;
        if (_currentAnimationIndex != animationIndex)
        {
            Start(animationIndex);
            _timesLoopedCurrentAnimation = 0;
        }
    }

    /// <summary>
    /// Pauses the current animation
    /// </summary>
    public void Pause() => _isPlaying = false;

    /// <summary>
    /// Stops playback if the current animation matches the specified index
    /// </summary>
    public void Stop(AnimationLibraryType animationIndex)
    {
        if (animationIndex.Equals(_currentAnimationIndex))
        {
            Stop();
        }
    }

    /// <summary>
    /// Stops the current animation and resets playback state
    /// </summary>
    public void Stop()
    {
#if DEBUG
        Plugin.LogDebug($"Current animation of: {_currentAnimationIndex.ToString()} stopped.");
#endif

        _currentAnimation?.Stop(owner);
        ResetAnimationStateFull();
    }

    // --- Private Methods ---
    private void Start(AnimationLibraryType animationIndex)
    {
        if (!animationLibrary.ContainsAnimation(animationIndex))
        {
            Plugin.LogWarning($"No Animation registered in animation library for animation of index: {animationIndex}, cannot start animation!");
            return;
        }

        _currentAnimationIndex = animationIndex;
        _currentAnimation = animationLibrary.GetAnimation(animationIndex);
        _currentAnimation.EmitSignal(_currentAnimation.animationStartedSignalEvent, owner);
        ResetAnimationStateBetweenLoops();

        _currentAnimation.Start(owner);
    }

    /// <summary>
    /// Updates the animation state and processes animation events
    /// </summary>
    public void Update()
    {
        if (!_isPlaying || _currentAnimation == null)
            return;

        _animationTimer++;

        _currentAnimation.Update(owner, _animationTimer);
        ProcessAnimationEvents();
        CheckAnimationCompletion();
    }

    /// <summary>
    /// Handles rendering updates for the current animation
    /// </summary>
    public void GraphicsUpdate()
    {
        if (_isPlaying && _currentAnimation != null)
        {
            _currentAnimation.GraphicsUpdate(owner, _animationTimer);
        }
    }

    private void ProcessAnimationEvents()
    {
        if (_currentAnimation.timeEvents.Length == 0)
            return;

        var nextEvent = _currentAnimation.timeEvents[_waitingForAnimationEventIndex];
        if (nextEvent.time <= _animationTimer)
        {
            nextEvent.method(owner);
            ContinueToNextAnimationEvent();
        }
    }

    private void CheckAnimationCompletion()
    {
        if (_animationTimer > _currentAnimation.length)
        {
            if (_currentAnimation.loop)
            {
                LoopAnimation();
            }
            else
            {
                FinishAnimation();
            }
        }
    }

    private void ContinueToNextAnimationEvent()
    {
        _waitingForAnimationEventIndex++;
    }

    private void LoopAnimation()
    {
        var timesLoop = _timesLoopedCurrentAnimation++;
        Start(_currentAnimationIndex);

        _currentAnimation?.EmitSignal(_currentAnimation.animationFinishedSignalEvent, owner);
        AnimationLooped?.Invoke(_currentAnimationIndex, _timesLoopedCurrentAnimation);
    }

    private void FinishAnimation()
    {
        var finishedAnim = _currentAnimation;
        var finishedAnimIndex = _currentAnimationIndex;
        var timesLooped = _timesLoopedCurrentAnimation;
        Stop();

        finishedAnim?.EmitSignal(finishedAnim.animationFinishedSignalEvent, owner);
        AnimationFinished?.Invoke(finishedAnimIndex, timesLooped);
    }

    private void ResetAnimationStateBetweenLoops()
    {
        _waitingForAnimationEventIndex = 0;
        _animationTimer = 0;
    }

    private void ResetAnimationStateFull()
    {
        ResetAnimationStateBetweenLoops();

        _currentAnimationIndex = null;
        _currentAnimation = null;
        _isPlaying = false;
        _timesLoopedCurrentAnimation = 0;
    }
}