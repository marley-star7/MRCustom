using System;
using UnityEngine;

namespace MRCustom.Extensions;

public static class BodyChunkConnectionExtensions
{
    /// <summary>
    /// Returns true if the connected body chunks are further apart then their set distance
    /// </summary>
    /// <param name="bodyChunkConnection"></param>
    /// <returns></returns>
    public static bool IsOverDistance(this Creature.BodyChunkConnection bodyChunkConnection)
    {
        var distBetweenChunks = Vector2.Distance(bodyChunkConnection.chunk1.pos, bodyChunkConnection.chunk2.pos);
        if (distBetweenChunks > bodyChunkConnection.distance)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Tugs on sent chunk as if it was park of the body chunk connection.
    /// </summary>
    /// <param name="bodyChunkConnection"></param>
    /// <param name="chunkToTug"></param>
    public static void TugOnChunk(this Creature.BodyChunkConnection bodyChunkConnection, BodyChunk chunkToTug)
    {
        // MS7: Don't ask me how this math works I don't know lol, I trial and errored after copying this code from BodyChunkConnections.

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
