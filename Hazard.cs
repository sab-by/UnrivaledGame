using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnrivaledPractise
{
    public class Hazard
    {
        public int Health;
        public bool isADummy;
        public bool canCollide;
        public Texture2D Texture;
        public Vector2 Position;
        public int hazardNum;
        public Rectangle shootingRange = new Rectangle(0, 230, 1600, 150);


        float time = 0f;
        public Random rand = new Random();

        public Hazard(int health, Texture2D texture, Vector2 position)
        {
            Health = health;
            Texture = texture;
            Position = position;
        }
        public void Update(GameTime gameTime, List<Hazard> hazards, List<Sprite> sprites)
        {

            if (Health > 0)
            {
                float loadTime = (float)rand.Next(3, 10);

                time += (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (var sprite in sprites)
                {
                    foreach (var hazard in hazards)
                    {
                        if (sprite.Rectangle.Intersects(shootingRange))
                        {

                            if (time > loadTime)
                            {
                                if (!hazard.isADummy)
                                {
                                    if (hazard.hazardNum == 2)
                                    {
                                        Debug.WriteLine("a");
                                        BulletManager.FireBullet(gameTime, hazard);
                                    }
                                    else
                                    {
                                        Debug.WriteLine("a2");
                                        BulletManager.FireBullet(gameTime, hazard);
                                    }
                                    time = 0f;
                                }
                            }
                        }

                    }
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Health > 0)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}
