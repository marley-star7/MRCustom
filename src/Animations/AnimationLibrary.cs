namespace MRCustom.Animations
{
    /// <summary>
    /// A collection of registered animations to be put in an animationPlayer.
    /// </summary>
    /// <typeparam name="ExtEnumType">The ExtEnum type used for animation keys</typeparam>
    /// <typeparam name="OwnerType">The type of object that can play these animations</typeparam>
    public class AnimationLibrary<ExtEnumType, OwnerType> 
        where ExtEnumType : ExtEnum<ExtEnumType> 
        where OwnerType : PhysicalObject
    {
        private readonly Dictionary<ExtEnumType, MRAnimation<OwnerType>> _animations = new();

        /// <summary>
        /// Registers a new animation with an auto-generated ExtEnum key
        /// </summary>
        /// <param name="animationName">Name for the new animation key</param>
        /// <param name="animation">Animation to register</param>
        /// <returns>The generated ExtEnum key</returns>
        public ExtEnumType RegisterAnimation(string animationName, MRAnimation<OwnerType> animation)
        {
            var animationKey = (ExtEnumType)Activator.CreateInstance(
                typeof(ExtEnumType),
                animationName,
                true); // register=true

            _animations.Add(animationKey, animation);
            return animationKey;
        }

        /// <summary>
        /// Registers an animation with an existing ExtEnum key
        /// </summary>
        /// <param name="animationIndex">Existing ExtEnum key</param>
        /// <param name="animation">Animation to register</param>
        public void RegisterAnimation(ExtEnumType animationIndex, MRAnimation<OwnerType> animation)
        {
            _animations.Add(animationIndex, animation);
        }

        /// <summary>
        /// Attempts to get an animation by its key
        /// </summary>
        /// <param name="key">Animation key to look up</param>
        /// <param name="animation">Output animation if found</param>
        /// <returns>True if animation was found</returns>
        public bool TryGetAnimation(ExtEnumType key, out MRAnimation<OwnerType> animation)
        {
            return _animations.TryGetValue(key, out animation);
        }

        /// <summary>
        /// Gets an animation by its key
        /// </summary>
        /// <param name="key">Animation key to look up</param>
        /// <returns>The found animation</returns>
        public MRAnimation<OwnerType> GetAnimation(ExtEnumType key)
        {
            if (_animations.TryGetValue(key, out var animation))
            {
                return animation;
            }
            Plugin.LogError($"Animation {key} not found in library");
            return null;
        }

        /// <summary>
        /// Checks if the library contains an animation
        /// </summary>
        /// <param name="key">Animation key to check</param>
        /// <returns>True if animation exists</returns>
        public bool ContainsAnimation(ExtEnumType key)
        {
            return _animations.ContainsKey(key);
        }

        /// <summary>
        /// Unregisters an animation from the library
        /// </summary>
        /// <param name="key">Animation key to remove</param>
        /// <returns>True if animation was found and removed</returns>
        public bool UnregisterAnimation(ExtEnumType key)
        {
            if (_animations.Remove(key))
            {
                key.Unregister();
                return true;
            }
            return false;
        }
    }
}