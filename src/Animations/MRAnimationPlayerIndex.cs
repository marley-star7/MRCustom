/*
namespace MRCustom.Animations;

/// <summary>
/// Class that is simply used to easily store the index for type animations.
/// </summary>
/// <typeparam name="T"></typeparam>
public class MRAnimationPlayerIndex<T> where T : PhysicalObject
{
    private Dictionary<string, MRAnimation<T>> _animations = new();
    // Readonly
    public Dictionary<string, MRAnimation<T>> Animations
    {
        get { return _animations; }
    }

    public void Register(MRAnimation<T> animation)
    {
        _animations.Add(animation.Name, animation);
    }
}
*/
