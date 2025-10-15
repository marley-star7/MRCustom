namespace MRCustom.Physics;

public class LineGraphics
{
    public LinePhysics linePhysics;
    public int spriteIndex = 0;

    public LineGraphics(LinePhysics linePhysics, int spriteIndex)
    {
        this.linePhysics = linePhysics;
        this.spriteIndex = spriteIndex;
    }

    public void Update()
    {

    }

    /// <summary>
    /// Initializes the sprite for the specified sprite leaser and room camera.
    /// </summary>
    /// <remarks>This method assigns a new triangle mesh to the sprite at the specified index within the
    /// sprite leaser. The mesh is configured based on the number of parts in the line physics and supports custom
    /// colors.</remarks>
    /// <param name="sLeaser">The <see cref="RoomCamera.SpriteLeaser"/> that manages the sprite to be initialized.</param>
    /// <param name="rCam">The <see cref="RoomCamera"/> associated with the sprite being initialized.</param>
    public void InitiateSprite(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites[spriteIndex] = TriangleMesh.MakeLongMesh(linePhysics.parts.Length, pointyTip: false, customColor: true);
    }

    /// <summary>
    /// Draw the sprite by sending over your sLeaser, reads from spriteIndex to modify.
    /// </summary>
    /// <param name="sLeaser"></param>
    /// <param name="rCam"></param>
    /// <param name="timeStacker"></param>
    /// <param name="camPos"></param>
    public void DrawSprite(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        var lineTriMesh = sLeaser.sprites[spriteIndex] as TriangleMesh;

        if (lineTriMesh == null)
        {
            return;
        }

        Vector2 startingStalksChangeInPos = Vector2.Lerp(linePhysics.parts[0].lastPos, linePhysics.parts[0].pos, timeStacker);
        startingStalksChangeInPos += Custom.DirVec(Vector2.Lerp(linePhysics.parts[1].lastPos, linePhysics.parts[1].pos, timeStacker), startingStalksChangeInPos) * linePhysics.partLength;

        for (int i = 0; i < linePhysics.parts.Length; i++)
        {
            Vector2 currentStalkPos = Vector2.Lerp(linePhysics.parts[i].lastPos, linePhysics.parts[i].pos, timeStacker);
            Vector2 normalized = (currentStalkPos - startingStalksChangeInPos).normalized;
            Vector2 currentStalkPerpindicularAngle = Custom.PerpendicularVector(normalized);
            float distanceFromFirstStalk = Vector2.Distance(currentStalkPos, startingStalksChangeInPos) / 5f;
            if (i == 0)
            {
                lineTriMesh.MoveVertice(i * 4, startingStalksChangeInPos - currentStalkPerpindicularAngle * linePhysics.parts[i].rad - camPos);
                lineTriMesh.MoveVertice(i * 4 + 1, startingStalksChangeInPos + currentStalkPerpindicularAngle * linePhysics.parts[i].rad - camPos);
            }
            else
            {
                lineTriMesh.MoveVertice(i * 4, startingStalksChangeInPos - currentStalkPerpindicularAngle * linePhysics.parts[i].rad + normalized * distanceFromFirstStalk - camPos);
                lineTriMesh.MoveVertice(i * 4 + 1, startingStalksChangeInPos + currentStalkPerpindicularAngle * linePhysics.parts[i].rad + normalized * distanceFromFirstStalk - camPos);
            }
            lineTriMesh.MoveVertice(i * 4 + 2, currentStalkPos - currentStalkPerpindicularAngle * linePhysics.parts[i].rad - normalized * distanceFromFirstStalk - camPos);
            lineTriMesh.MoveVertice(i * 4 + 3, currentStalkPos + currentStalkPerpindicularAngle * linePhysics.parts[i].rad - normalized * distanceFromFirstStalk - camPos);

            startingStalksChangeInPos = currentStalkPos;
        }
    }
}
