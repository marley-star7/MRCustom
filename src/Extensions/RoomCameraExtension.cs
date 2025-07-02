namespace MRCustom.Extensions;

public static class RoomCameraExtension
{
    public static float cameraZoom = 1f;
    public static Vector2 cameraOffset = Vector2.zero;

    /// <summary>
    /// Resets the camera's zoom to default values.
    /// </summary>
    /// <param name="room_camera"></param>
    public static void ResetCameraZoom(this RoomCamera roomCamera)
    {
        roomCamera.SetCameraZoom(1f);
    }

    /// <summary>
    /// Set the camera's zoom to a specific value.
    /// </summary>
    /// <param name="roomCamera"></param>
    /// <param name="zoom"></param>
    public static void SetCameraZoom(this RoomCamera roomCamera, float newCameraZoom)
    {
        cameraZoom = newCameraZoom;
    }
}
