namespace MRCustom.Modules.Weapons.Rotations;

public class SmallWeaponRotationModule : WeaponClassReferenceRotationModule
{
    public float rotationDegreesOffset = 0f;

    public float LastScaleX;
    public float ScaleX;

    public SmallWeaponRotationModule(Weapon owner) : base(owner)
    {
        ScaleX = 1;
    }

    protected override void LogicUpdate()
    {
        LastScaleX = ScaleX;

        if (Owner.mode == Weapon.Mode.Free)
        {
            // Give it the resting position whenever it makes contact.
            if (Owner.firstChunk.ContactPoint.y < 0 && Owner.firstChunk.lastContactPoint == IntVector2Extensions.zero)
            {
                Owner.rotationSpeed = 0f;
                Rotation = Custom.DegToVec(Mathf.Lerp(-50f, 50f, UnityEngine.Random.value) + 180f);
                // TODO: Should move this below here to somewhere better.
                Owner.room.PlaySound(SoundID.Spear_Stick_In_Ground, Owner.firstChunk);
            }
            ScaleX = 1;

            return;
        }

        if (Owner.mode == Weapon.Mode.Thrown)
        {
            ScaleX = MarMath.NonzeroSign(Owner.throwDir.x);

            return;
        }

        if (Owner.mode == Weapon.Mode.Carried)
        {
            if (Owner.grabbedBy.Count > 0 && Owner.grabbedBy[0].grabber is Player playerGrabber)
            {
                var rotationXInfluence = 0f;

                if ((playerGrabber.bodyMode == Player.BodyModeIndex.Stand && playerGrabber.input[0].x != 0))
                {
                    rotationXInfluence = playerGrabber.input[0].x;
                    ScaleX = playerGrabber.input[0].x;
                }
                else if (playerGrabber.bodyMode == Player.BodyModeIndex.Crawl)
                {
                    var bodyToHeadDirSignX = Mathf.Sign(playerGrabber.bodyChunks[Consts.BodyChunkIndexes.Player.Head].pos.x - playerGrabber.bodyChunks[Consts.BodyChunkIndexes.Player.Body].pos.x);

                    rotationXInfluence = bodyToHeadDirSignX;
                    ScaleX = bodyToHeadDirSignX;
                }
                else
                {
                    float bodyToHandDirX = ((PlayerGraphics)playerGrabber.graphicsModule).hands[Owner.grabbedBy[0].graspUsed].pos.x - playerGrabber.bodyChunks[Consts.BodyChunkIndexes.Player.Head].pos.x;
                    bodyToHandDirX = Mathf.Clamp(bodyToHandDirX, -1.5f, 1.5f) / 1.5f; // We use 2 for more accurate placement.

                    ScaleX = bodyToHandDirX;

                    // Ms7: This was a better method that WOULD solved the flipping on slide jumping if it wasn't totally broken.
                    // Ugh, figure out how to fix it later.

                    /*
                    var distBodyFromHand = Vector2.Distance(((PlayerGraphics)playerGrabber.graphicsModule).hands[Owner.grabbedBy[0].graspUsed].pos, playerGrabber.bodyChunks[Consts.BodyChunkIndexes.Player.Body].pos);
                    if (distBodyFromHand < 1.5f) // If hand is too close to body, do the flip into itself thing.
                    {
                        var directionBodyToHandX = ((PlayerGraphics)playerGrabber.graphicsModule).hands[Owner.grabbedBy[0].graspUsed].pos.x - playerGrabber.bodyChunks[Consts.BodyChunkIndexes.Player.Body].pos.x;

                        ScaleX = Mathf.InverseLerp(0, 1.5f, distBodyFromHand) * Mathf.Sign(directionBodyToHandX);
                    }
                    else
                    {
                        ScaleX = MarMath.NonzeroSign(ScaleX);
                    }
                    */
                }

                // Face the direction 
                var newRotation = Rotation;
                newRotation = Custom.DirVec(
                    playerGrabber.bodyChunks[Consts.BodyChunkIndexes.Player.Head].pos,
                    ((PlayerGraphics)playerGrabber.graphicsModule).hands[Owner.grabbedBy[0].graspUsed].pos
                );
                newRotation.x += rotationXInfluence;
                newRotation = Custom.rotateVectorDeg(newRotation, rotationDegreesOffset * ScaleX);

                // The closer rotation.X is to 0, the bigger to 1, to fake effect of turning to camera.
                Rotation = newRotation.normalized;
            }

            return;
        }
    }
}
