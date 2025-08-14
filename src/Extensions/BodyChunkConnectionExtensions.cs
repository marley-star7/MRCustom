using System;

namespace MRCustom.Extensions;

public static class BodyChunkConnectionExtensions
{
    public static bool IsOverDistance(this Creature.BodyChunkConnection bodyChunkConnection)
    {
        var distBetweenChunks = Vector2.Distance(bodyChunkConnection.chunk1.pos, bodyChunkConnection.chunk2.pos);
        if (distBetweenChunks > bodyChunkConnection.distance)
            return true;
        else
            return false;
    }

    // MS7: Don't ask me how this math works I don't know lol, I trial and errored after copying this code from BodyChunkConnections.
    public static void TugOnChunk(this Creature.BodyChunkConnection bodyChunkConnection, BodyChunk chunkToTug)
    {
        if (!bodyChunkConnection.active)
            return;

        var distBetweenChunks = Vector2.Distance(bodyChunkConnection.chunk1.pos, bodyChunkConnection.chunk2.pos);

        if (bodyChunkConnection.type == Creature.BodyChunkConnection.Type.Normal 
            || (bodyChunkConnection.type == Creature.BodyChunkConnection.Type.Pull && distBetweenChunks > bodyChunkConnection.distance) 
            || (bodyChunkConnection.type == Creature.BodyChunkConnection.Type.Push && distBetweenChunks < bodyChunkConnection.distance))
        {
            Vector2 vector = Custom.DirVec(bodyChunkConnection.chunk1.pos, bodyChunkConnection.chunk2.pos);
            chunkToTug.pos -= (bodyChunkConnection.distance - distBetweenChunks) * vector * bodyChunkConnection.weightSymmetry * bodyChunkConnection.elasticity;
            chunkToTug.vel -= (bodyChunkConnection.distance - distBetweenChunks) * vector * bodyChunkConnection.weightSymmetry * bodyChunkConnection.elasticity;
        }
    }
}
