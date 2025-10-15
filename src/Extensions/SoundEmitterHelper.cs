namespace MRCustom.Extensions;

public static class SoundEmitterHelper
{
    public static bool IsActive(SoundEmitter? chunkSoundEmitter)
    {
        if (chunkSoundEmitter == null
            || chunkSoundEmitter.currentSoundObject == null
            || chunkSoundEmitter.currentSoundObject.slatedForDeletion)
            return false;

        return true;
    }
}