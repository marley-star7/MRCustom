using static Player;

namespace MRCustom.Extensions;

public static class PlayerExtensions
{
    /// <summary>
    /// Switch the items in the player's hands.
    /// Stolen from source.
    /// </summary>
    /// <param name="player"></param>
    public static void SwitchHands(this Player player)
    {
        if (player.input[0].pckp && !player.input[1].pckp && player.switchHandsProcess == 0f && !player.isSlugpup)
        {
            bool flag4 = player.grasps[0] != null || player.grasps[1] != null;
            if (player.grasps[0] != null && (player.Grabability(player.grasps[0].grabbed) == ObjectGrabability.TwoHands || player.Grabability(player.grasps[0].grabbed) == ObjectGrabability.Drag))
            {
                flag4 = false;
            }
            if (flag4)
            {
                if (player.switchHandsCounter == 0)
                {
                    player.switchHandsCounter = 15;
                }
                else
                {
                    player.room.PlaySound(SoundID.Slugcat_Switch_Hands_Init, player.mainBodyChunk);
                    player.switchHandsProcess = 0.01f;
                    player.wantToPickUp = 0;
                    player.noPickUpOnRelease = 20;
                }
            }
            else
            {
                player.switchHandsProcess = 0f;
            }
        }
    }

    /// <summary>
    /// Gets the current threat level of the player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static float GetThreat(this Player player)
    {
        if (player == null)
            return 0f;
        if (player.abstractCreature.world.game.GameOverModeActive)
            return 0f;
        if (player.abstractCreature.world.game.manager.musicPlayer != null && player.abstractCreature.world.game.manager.musicPlayer.threatTracker != null)
            return player.abstractCreature.world.game.manager.musicPlayer.threatTracker.currentMusicAgnosticThreat;
        if (player.abstractCreature.world.game.manager.fallbackThreatDetermination == null)
            player.abstractCreature.world.game.manager.fallbackThreatDetermination = new ThreatDetermination(0);

        return player.abstractCreature.world.game.manager.fallbackThreatDetermination.currentMusicAgnosticThreat;
    }
}
