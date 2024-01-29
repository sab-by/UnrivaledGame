using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UnrivaledPractise
{
    class Bullet
    {
        public Vector2 Position;
        public Vector2 Velocity = new Vector2(5,0);
        public bool Active;
        public bool moveLeft;
        Texture2D Texture;
        public void Initialize(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Active = true;
        }

        public void Update(GameTime gameTime, Sprite sprite)
        {
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
