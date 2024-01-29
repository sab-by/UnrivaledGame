using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UnrivaledPractise
{
    class Flag
    {
        public static Texture2D Texture;
        public static Vector2 Position = new Vector2(800, 830);
        public static bool isPickedUpP1 = false;
        public static bool isPickedUpP2 = false;
        public static void Initialize(Texture2D texture)
        {
            Texture = texture;
        }

        public static void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Rectangle flagRect = new Rectangle((int)Position.X, (int)Position.Y, 33, 50);
            foreach (var sprite in sprites)
            {
                if (sprite.Rectangle.Intersects(flagRect))
                {
                    if (sprite.PlayerNum == 1 && !isPickedUpP2)
                    {
                        flagRect.X = (int)sprite.Position.X;
                        flagRect.Y = (int)sprite.Position.Y;
                        Position = new Vector2(flagRect.X, flagRect.Y);
                        isPickedUpP1 = true;
                    }
                    if (sprite.PlayerNum != 1 && !isPickedUpP1)
                    {
                        flagRect.X = (int)sprite.Position.X;
                        flagRect.Y = (int)sprite.Position.Y;
                        Position = new Vector2(flagRect.X, flagRect.Y);
                        isPickedUpP2 = true;
                    }
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
