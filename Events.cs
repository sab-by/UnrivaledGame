using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace UnrivaledPractise
{
    static class Events
    {
       public static bool FlagCapP1 = false;
        public static bool FlagCapP2 = false;


        public static void Initialize()
        {

        }

        public static void Update(GameTime gametime, List<Sprite> sprites, List<Rectangle> p1Tiles, List<Rectangle> p2Tiles)
        {

            foreach (var sprite in sprites)
            {
                foreach (var tile in p1Tiles)
                {
                    if (sprite.PlayerNum == 1 && (Sprite.IsTouchingTileLeft(sprite, tile) || Sprite.IsTouchingTileRight(sprite, tile)) && Flag.isPickedUpP1)
                    {
                        FlagCapP2 = true;
                    }
                }
                foreach (var tile in p2Tiles)
                {
                    if (sprite.PlayerNum != 1 && (Sprite.IsTouchingTileLeft(sprite, tile) || Sprite.IsTouchingTileRight(sprite, tile)) && Flag.isPickedUpP2)
                    {
                        FlagCapP1 = true;
                    }
                }

                if (sprite.Health == 0)
                {
                    if (sprite.PlayerNum == 1)
                    {

                    }
                }
            }
        }

        public static void Draw()
        {

        }
    }
}
