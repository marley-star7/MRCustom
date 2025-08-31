namespace MRCustom;

public static class Enums
{
    public static class SoundID
    {
        public static global::SoundID Clack_Small;
        public static global::SoundID Clack_Done;

        internal static void Initialize()
        {
            Clack_Small = new global::SoundID("Clack_Small", true);
            Clack_Done = new global::SoundID("Clack_Done", true);
        }
    }
}
