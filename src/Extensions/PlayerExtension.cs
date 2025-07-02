using static Player;

namespace MRCustom.Extensions;

public static class PlayerExtension
{
    /// <summary>
    /// Stolen from source.
    /// </summary>
    /// <param name="selfPlayer"></param>
    public static void SwitchHands(this Player selfPlayer)
    {
        if (selfPlayer.input[0].pckp && !selfPlayer.input[1].pckp && selfPlayer.switchHandsProcess == 0f && !selfPlayer.isSlugpup)
        {
            bool flag4 = selfPlayer.grasps[0] != null || selfPlayer.grasps[1] != null;
            if (selfPlayer.grasps[0] != null && (selfPlayer.Grabability(selfPlayer.grasps[0].grabbed) == ObjectGrabability.TwoHands || selfPlayer.Grabability(selfPlayer.grasps[0].grabbed) == ObjectGrabability.Drag))
            {
                flag4 = false;
            }
            if (flag4)
            {
                if (selfPlayer.switchHandsCounter == 0)
                {
                    selfPlayer.switchHandsCounter = 15;
                }
                else
                {
                    selfPlayer.room.PlaySound(SoundID.Slugcat_Switch_Hands_Init, selfPlayer.mainBodyChunk);
                    selfPlayer.switchHandsProcess = 0.01f;
                    selfPlayer.wantToPickUp = 0;
                    selfPlayer.noPickUpOnRelease = 20;
                }
            }
            else
            {
                selfPlayer.switchHandsProcess = 0f;
            }
        }
    }
}
