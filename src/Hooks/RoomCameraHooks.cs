using MRCustom.Extensions;

namespace MRCustom.Hooks;

public static class RoomCameraHooks
{
    internal static void ApplyHooks()
    {
        On.RoomCamera.DrawUpdate += RoomCamera_DrawUpdate;
    }

    internal static void RemoveHooks()
    {
        On.RoomCamera.DrawUpdate -= RoomCamera_DrawUpdate;
    }

    //-- MR7: Taken from "Slugcat Eyebrow Raise" mod.
    // I assume from the comment stolen below however, that this code was written by Henpemaz, so credit to him for this.
    // Although, I have changed it to no longer focus on the player.

    // Henpemaz's magic

    private static void RoomCamera_DrawUpdate(On.RoomCamera.orig_DrawUpdate orig, RoomCamera roomCamera, float timeStacker, float timeSpeed)
    {
        var zoom = RoomCameraExtension.cameraZoom;
        var offset = RoomCameraExtension.cameraOffset;

        #region Follow & Zoom

        // Zoom in
        if (zoom != 1f)
        {
            // 11 useful layers, the reset is HUD
            for (var i = 0; i < 11; i++)
            {
                roomCamera.SpriteLayers[i].scale = 1.0f;
                roomCamera.SpriteLayers[i].SetPosition(Vector2.zero);
                roomCamera.SpriteLayers[i].ScaleAroundPointRelative(roomCamera.sSize / 2f, zoom, zoom);
            }
            roomCamera.offset = offset;
        }
        else
        {
            // Unzoom camera on effect slider to 0 or maybe if ChangeRoom didnt call
            for (var i = 0; i < 11; i++)
            {
                roomCamera.SpriteLayers[i].scale = 1f;
                roomCamera.SpriteLayers[i].SetPosition(Vector2.zero);
            }
            roomCamera.offset = new Vector2(roomCamera.cameraNumber * 6000.0f, 0.0f);
        }


        var randomSeed = 0;

        if (zoom != 1f)
        {
            // deterministic random shake
            randomSeed = Random.seed;
            Random.seed = randomSeed;
        }

        orig(roomCamera, timeStacker, timeSpeed);

        #endregion

        if (zoom != 1f)
        {
            Random.seed = randomSeed;
            var shakeOffset = Vector2.Lerp(roomCamera.lastPos, roomCamera.pos, timeStacker);

            if (roomCamera.microShake > 0f)
            {
                shakeOffset += Custom.RNV() * 8f * roomCamera.microShake * Random.value;
            }

            if (!roomCamera.voidSeaMode)
            {
                shakeOffset.x = Mathf.Clamp(shakeOffset.x, roomCamera.CamPos(roomCamera.currentCameraPosition).x + roomCamera.hDisplace + 8f - 20f, roomCamera.CamPos(roomCamera.currentCameraPosition).x + roomCamera.hDisplace + 8f + 20f);
                shakeOffset.y = Mathf.Clamp(shakeOffset.y, roomCamera.CamPos(roomCamera.currentCameraPosition).y + 8f - 7f, roomCamera.CamPos(roomCamera.currentCameraPosition).y + 33f);
            }
            else
            {
                shakeOffset.y = Mathf.Min(shakeOffset.y, -528f);
            }

            shakeOffset = new Vector2(Mathf.Floor(shakeOffset.x), Mathf.Floor(shakeOffset.y));
            shakeOffset.x -= 0.02f;
            shakeOffset.y -= 0.02f;

            var magicOffset = roomCamera.CamPos(roomCamera.currentCameraPosition) - shakeOffset;
            var textureOffset = shakeOffset + magicOffset;

            //Vector4 center = new Vector4(
            //	(-shakeOffset.x - 0.5f + roomCamera.levelGraphic.width / 2f + roomCamera.CamPos(roomCamera.currentCameraPosition).x) / roomCamera.sSize.x,
            //	(-shakeOffset.y + 0.5f + roomCamera.levelGraphic.height / 2f + roomCamera.CamPos(roomCamera.currentCameraPosition).y) / roomCamera.sSize.y,
            //	(-shakeOffset.x - 0.5f + roomCamera.levelGraphic.width / 2f + roomCamera.CamPos(roomCamera.currentCameraPosition).x) / roomCamera.sSize.x,
            //	(-shakeOffset.y + 0.5f + roomCamera.levelGraphic.height / 2f + roomCamera.CamPos(roomCamera.currentCameraPosition).y) / roomCamera.sSize.y);

            var center = new Vector4(
                (magicOffset.x + roomCamera.levelGraphic.width / 2f) / roomCamera.sSize.x,
                (magicOffset.y + 2f + roomCamera.levelGraphic.height / 2f) / roomCamera.sSize.y,
                (magicOffset.x + roomCamera.levelGraphic.width / 2f) / roomCamera.sSize.x,
                (magicOffset.y + 2f + roomCamera.levelGraphic.height / 2f) / roomCamera.sSize.y);

            shakeOffset += roomCamera.offset;

            var spriteRectPos = new Vector4(
                (-shakeOffset.x + textureOffset.x) / roomCamera.sSize.x,
                (-shakeOffset.y + textureOffset.y) / roomCamera.sSize.y,
                (-shakeOffset.x + roomCamera.levelGraphic.width + textureOffset.x) / roomCamera.sSize.x,
                (-shakeOffset.y + roomCamera.levelGraphic.height + textureOffset.y) / roomCamera.sSize.y);

            //spriteRectPos -= new Vector4(17f / roomCamera.sSize.x, 18f / roomCamera.sSize.y, 17f / roomCamera.sSize.x, 18f / roomCamera.sSize.y) * (1f - 1f / zoom);

            spriteRectPos -= center;
            spriteRectPos *= zoom;
            spriteRectPos += center;

            Shader.SetGlobalVector("_spriteRect", spriteRectPos);


            if (roomCamera.room is not null)
            {
                var zooming = (1f - 1f / zoom) * new Vector2(roomCamera.sSize.x / roomCamera.room.PixelWidth, roomCamera.sSize.y / roomCamera.room.PixelHeight);

                Shader.SetGlobalVector("_camInRoomRect", new Vector4(
                    shakeOffset.x / roomCamera.room.PixelWidth + zooming.x / 2f,
                    shakeOffset.y / roomCamera.room.PixelHeight + zooming.y / 2f,
                    roomCamera.sSize.x / roomCamera.room.PixelWidth - zooming.x,
                    roomCamera.sSize.y / roomCamera.room.PixelHeight - zooming.y));
            }

            Shader.SetGlobalVector("_screenSize", roomCamera.sSize);
        }
    }
}