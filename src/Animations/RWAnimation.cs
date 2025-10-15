namespace MRCustom.Animations;

public abstract class RWAnimation<T> where T : PhysicalObject
{
    public delegate void AnimationEventDelegate<T>(T player) where T : PhysicalObject;

    /// <summary>
    /// Built in signal events that always plays on animation start.
    /// </summary>
    public StringName animationStartedSignalEvent = new StringName("AnimationStarted");
    /// <summary>
    /// Built in signal events that always plays on animation finish.
    /// </summary>
    public StringName animationFinishedSignalEvent = new StringName("AnimationFinished");

    public struct TimeEvent<T2> where T2 : PhysicalObject
    {
        public float time;
        public AnimationEventDelegate<T2> method;

        public TimeEvent(float time, AnimationEventDelegate<T2> method)
        {
            this.time = time;
            this.method = method;
        }
    }

    /// <summary>
    /// Events which play at a specific time.
    /// </summary>
    public TimeEvent<T>[] timeEvents = new TimeEvent<T>[0];
    /// <summary>
    /// Events which play on the output of a specific signal by the animation.
    /// </summary>
    public Dictionary<StringName, List<AnimationEventDelegate<T>>> signalEvents = new();

    /// <summary>
    /// The length of the animation
    /// If set to -1, animation will play until told to stop.
    /// </summary>
    public float length = -1;

    /// <summary>
    /// Wether or not the animation loops on completion.
    /// </summary>
    public bool loop = true;

    public RWAnimation()
    {

    }

    /// <summary>
    /// Start the animation.
    /// </summary>
    /// <param name="owner"></param>
    public abstract void Start(T owner);
    /// <summary>
    /// Stop the animation.
    /// </summary>
    /// <param name="owner"></param>
    public abstract void Stop(T owner);
    /// <summary>
    /// Ran on normal update.
    /// </summary>
    /// <param name="animationTime"></param>
    public abstract void Update(T owner, float animationTimer);
    /// <summary>
    /// Ran on graphics update.
    /// </summary>
    /// <param name="animationTime"></param>
    public abstract void GraphicsUpdate(T owner, float animationTimer);

    public void EmitSignal(StringName signal, T owner)
    {
        if (signalEvents.ContainsKey(signal))
        {
            var thisSignalsEvents = signalEvents[signal];

            for (int i = 0; i < thisSignalsEvents.Count; i++)
            {
                thisSignalsEvents[i].Invoke(owner);
            }
        }
    }

    public void AddSignalEvent(StringName signal, AnimationEventDelegate<T> handler)
    {
        if (!signalEvents.TryGetValue(signal, out var handlers))
        {
            handlers = new List<AnimationEventDelegate<T>>();
            signalEvents[signal] = handlers;
        }
        handlers.Add(handler);
    }
}