using RWCustom;
using UnityEngine;

namespace EthansUniverse.Debug
{
    public class DebugLimbVisualizer
    {
        BodyChunk chunk;
        Limb limb;
        Room room;
        DebugSprite[] sprites;

        public DebugLimbVisualizer(Limb limbToVisualize, BodyChunk rootChunk, Room curRoom)
        {
            limb = limbToVisualize;
            chunk = rootChunk;
            room = curRoom;

            sprites = new DebugSprite[]
            {
                new DebugSprite(Vector2.zero, new FSprite("pixel"), room),
                new DebugSprite(Vector2.zero, new FSprite("pixel"), room),
                new DebugSprite(Vector2.zero, new FSprite("pixel"), room),
            };

            foreach (DebugSprite sprite in sprites)
            {
                sprite.sprite.color = Color.red;
                sprite.sprite.scaleX = 5f;
                sprite.sprite.scaleY = 20;
                sprite.sprite.anchorX = 0.5f;
                sprite.sprite.anchorY = 0;
                room.AddObject(sprite);
            }

            sprites[0].sprite.scaleX = 3;
        }

        public void Update(float timeStacker, float limbLength, Vector2 offset)
        {
            Vector2 lerpedPos = Vector2.Lerp(chunk.lastPos + offset, chunk.pos + offset, timeStacker);

            sprites[1].pos = lerpedPos;
            sprites[1].sprite.rotation = Custom.AimFromOneVectorToAnother(lerpedPos, limb.absoluteHuntPos);
            sprites[1].sprite.scaleY = Vector2.Distance(lerpedPos, limb.absoluteHuntPos);

            Vector2 ik = Custom.InverseKinematic(lerpedPos, limb.pos, limbLength / 2, limbLength / 2, Mathf.Lerp(1f, -1f, Mathf.Clamp01(chunk.vel.x)));

            sprites[0].pos = lerpedPos;
            sprites[0].sprite.rotation = Custom.AimFromOneVectorToAnother(lerpedPos, ik);
            sprites[0].sprite.scaleY = Mathf.Clamp(Vector2.Distance(lerpedPos, ik), 0f, limbLength / 2);
            sprites[0].sprite.color = Color.magenta;
            sprites[0].sprite.MoveInFrontOfOtherNode(sprites[1].sprite);

            sprites[2].pos = ik;
            sprites[2].sprite.rotation = Custom.AimFromOneVectorToAnother(ik, limb.pos);
            sprites[2].sprite.scaleY = Mathf.Clamp(Vector2.Distance(ik, limb.pos), 0f, limbLength / 2);
            sprites[2].sprite.color = Color.magenta;
            sprites[2].sprite.MoveInFrontOfOtherNode(sprites[1].sprite);

            if (limb.mode == Limb.Mode.Dangle || limb.mode == Limb.Mode.Retracted)
            {
                sprites[1].sprite.color = Color.blue;
            }

            else if (limb.mode == Limb.Mode.HuntRelativePosition || limb.mode == Limb.Mode.HuntAbsolutePosition)
            {
                if (limb.OverLappingHuntPos)
                {
                    sprites[1].sprite.color = Color.green;
                }

                else
                {
                    sprites[1].sprite.color = Color.red;
                }
            }
        }

        public void OnChangeRooms(Room newRoom)
        {
            foreach (DebugSprite sprite in sprites)
            {
                room.RemoveObject(sprite);
                newRoom.AddObject(sprite);
            }

            room = newRoom;
        }
    }
}
