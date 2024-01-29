using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnrivaledPractise.Gui;



namespace UnrivaledPractise
{
    class BulletManager 
    {

        static Texture2D bulletTextureLeft;
        static Texture2D bulletTextureRight;
        static Rectangle bulletRectangle;
        static public List<Bullet> Bullets;
        const float SECONDS_IN_MINUTE = 60f;
        const float RATE_OF_FIRE = 300f;

        static TimeSpan bulletSpawnTime = TimeSpan.FromSeconds(SECONDS_IN_MINUTE / RATE_OF_FIRE);
        static TimeSpan previousBulletSpawnTime;
        static Vector2 graphicsInfo;


        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public void Initialize(Texture2D texture, Texture2D texture2, GraphicsDevice Graphics)
        {
            Bullets = new List<Bullet>();
            previousBulletSpawnTime = TimeSpan.Zero;
            bulletTextureLeft = texture;
            bulletTextureRight = texture2;
            graphicsInfo.X = Graphics.Viewport.Width;
            graphicsInfo.Y = Graphics.Viewport.Height;
        }
        public static void FireBullet(GameTime gameTime, Sprite sprite)
        {
            if (gameTime.TotalGameTime - previousBulletSpawnTime > bulletSpawnTime)
            {
                previousBulletSpawnTime = gameTime.TotalGameTime;
                AddBullet(sprite);
            }
        }
        public static void FireBullet(GameTime gameTime, Hazard hazard)
        {
            if (gameTime.TotalGameTime - previousBulletSpawnTime > bulletSpawnTime)
            {
                previousBulletSpawnTime = gameTime.TotalGameTime;
                AddBullet(hazard);
            }
        }

        public static void AddBullet(Sprite sprite)
        {
            Bullet bullet = new Bullet();
            var bulletPosition = sprite.Position;
            if (sprite.isFacingLeft)
            {
                bullet.moveLeft = true;
                bulletPosition.Y += 30;
                bulletPosition.X -= 10;
                bullet.Initialize(bulletTextureLeft, bulletPosition);
            } else
            {
                bullet.moveLeft = false;
                bulletPosition.Y += 37;
                bulletPosition.X += 45;
                bullet.Initialize(bulletTextureRight, bulletPosition);
            }
            
            
            Bullets.Add(bullet);
        }

        public static void AddBullet(Hazard hazard)
        {
            Bullet bullet = new Bullet();
            var bulletPosition = hazard.Position;
                bullet.moveLeft = false;
                bulletPosition.Y += 37;
                bulletPosition.X += 45;
                bullet.Initialize(bulletTextureRight, bulletPosition);

            Bullets.Add(bullet);
        }

        public void UpdateManagerBullet(GameTime gameTime, List<Sprite> sprites, List<Hazard> hazards)
        {
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
          if (Game1.titlelock ==true) {
                foreach (var sprite in sprites)
                {
                    if (currentKeyboardState.IsKeyDown(Keys.End) && previousKeyboardState.IsKeyUp(Keys.End))
                    {
                        if (sprite.PlayerNum == 1)
                        {
                            FireBullet(gameTime, sprite);
                        }

                    }
                    if (currentKeyboardState.IsKeyDown(Keys.B) && previousKeyboardState.IsKeyUp(Keys.B))
                    {
                        if (sprite.PlayerNum != 1)
                        {
                            FireBullet(gameTime, sprite);
                        }

                    }

                    for (var i = 0; i < Bullets.Count; i++)
                    {
                        if (Bullets[i].moveLeft == true)
                        {
                            Bullets[i].Position -= Bullets[i].Velocity;
                        }
                        if (Bullets[i].moveLeft == false)
                        {
                            Bullets[i].Position += Bullets[i].Velocity;
                        }

                        if (!Bullets[i].Active || Bullets[i].Position.X > 1575 || Bullets[i].Position.X < 5)
                        {
                            Bullets.Remove(Bullets[i]);
                        }
                    }
                }



                foreach (var sprite in sprites)
                {
                    Rectangle spriteRect = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, 50, 90);

                    foreach (Bullet bullet in BulletManager.Bullets)
                    {
                        bulletRectangle = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, 12, 6);

                        if (bulletRectangle.Intersects(spriteRect))
                        {
                            sprite.Health -= 20;

                            bullet.Active = false;
                        }
                    }
                }
                foreach (var hazard in hazards)
                {
                    Rectangle hazardRect = new Rectangle((int)hazard.Position.X, (int)hazard.Position.Y, 50, 90);

                    foreach (Bullet bullet in BulletManager.Bullets)
                    {
                        bulletRectangle = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, 12, 6);

                        if (bulletRectangle.Intersects(hazardRect))
                        {
                            if (hazard.Health > 0 && hazard.canCollide)
                            {
                                hazard.Health -= 20;

                                bullet.Active = false;
                            }
                        }
                    }
                }
                previousKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();
            }
            
        }

        public void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
