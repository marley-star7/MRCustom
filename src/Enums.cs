using Fisobs.Sandbox;

namespace MRCustom;

public static class Enums
{
    public static void Init()
    {
        RuntimeHelpers.RunClassConstructor(typeof(SoundID).TypeHandle);
        RuntimeHelpers.RunClassConstructor(typeof(PlayerHandAnimations).TypeHandle);
    }

    public static class SoundID
    {
        public static global::SoundID Clack_Small = new global::SoundID("Clack_Small", true);
        public static global::SoundID Clack_Done = new global::SoundID("Clack_Done", true);
    }

    public static class PlayerHandAnimations
    {
        public static readonly PlayerHandAnimationPlayer.AnimationIndex KnifeStab = new PlayerHandAnimationPlayer.AnimationIndex("KnifeStab", register: true);

        internal static void RegisterValues()
        {
            PlayerHandAnimationPlayer.defaultPlayerHandAnimationLibrary.RegisterAnimation(KnifeStab,
                new WeaponStabPlayerAnimation()
                {
                    length = -1,
                }
            );
        }
    }
}
