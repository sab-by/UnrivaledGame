using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UnrivaledPractise
{
    public class Sprite
    {
        #region Declarations
        public Input Input;

        protected KeyboardState currentKeyboardState;
        protected KeyboardState previousKeyboardState;

        protected AnimationManager _animationManager;
        protected Texture2D Texture;
        protected bool hasJumped;
        public bool isFacingLeft;
        public bool isTouchingTile;

        public int Health;
        public int Lives;
        public int PlayerNum;
        public Vector2 HPPosition;
        public Vector2 LivesPosition;
        public Vector2 Velocity;
        public Vector2 Gravity = new Vector2(0, 20);

        public float LifeSpan = 0f;
        public bool IsRemoved = false;

        protected Dictionary<string, Animation> _animations;
        public Vector2 _position;


        public float Speed = 5f;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 50, 90);
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }
        #endregion

        #region Constructors
        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }
        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        #endregion

        #region Update, Move and Draw
        public virtual void Update(GameTime gameTime, List<Rectangle> Tiles, List<Rectangle> p1tiles, List<Rectangle> p2tiles, List<Sprite> sprites, List<Hazard> hazards)
        {
            isTouchingTile = false;
            SetAnimations(sprites);
            _animationManager.Update(gameTime);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();



            //Player Movement
            Position += Velocity;

            if (Input == null)
            {
                return;
            }

            //Move Left/Right
            if (currentKeyboardState.IsKeyDown(Input.Left))
            {
                isFacingLeft = true;
                Velocity.X = -Speed;
            }

            else if (currentKeyboardState.IsKeyDown(Input.Right))
            {
                Velocity.X = Speed;
                isFacingLeft = false;
            }

            else
                Velocity.X = 0;

            //Jump
            if (currentKeyboardState.IsKeyDown(Input.Up) && hasJumped == false)
            {
                Velocity.Y = -7.3f;
                hasJumped = true;
            }

            if (hasJumped == true)
            {
                float i = 1;
                Velocity.Y += 0.15f * i;
            }
            if (hasJumped == false)
            {
                Velocity.Y = 0f;
            }

            //Sprite Collision
            foreach (var sprite in sprites)
            {
                if (sprite == this)
                    continue;

                //Touching Left/Right Sprite
                if (this.Velocity.X > 0 && this.IsTouchingLeft(sprite) ||
                    this.Velocity.X < 0 && this.IsTouchingRight(sprite))
                {
                    this.Velocity.X = 0;
                }
                //Touching Up/Down Sprite
                if (this.Velocity.Y > 0 && this.IsTouchingTop(sprite) ||
                    this.Velocity.Y < 0 && this.IsTouchingBottom(sprite))
                {
                    this.Velocity.Y = 0;
                    hasJumped = false;
                }
                else
                    hasJumped = true;
            }

            //Hazard Colision
            foreach (var hazard in hazards)
            { 
                foreach (var sprite in sprites)
                {
                    if (sprite == this)
                        continue;
                    if (hazard.Health > 0 && hazard.canCollide)
                    {
                        //Touching Left/Right Sprite
                        if (this.Velocity.X > 0 && this.IsTouchingHazardLeft(sprite, hazard) ||
                            this.Velocity.X < 0 && this.IsTouchingHazardRight(sprite, hazard))
                        {
                            this.Velocity.X = 0;
                        }
                        //Touching Up/Down Sprite
                        if (this.Velocity.Y > 0 && this.IsTouchingHazardTop(sprite, hazard) ||
                            this.Velocity.Y < 0 && this.IsTouchingHazardBottom(sprite, hazard))
                        {
                            this.Velocity.Y = 0;
                            hasJumped = false;
                        }
                        else
                            hasJumped = true;
                    }
                }
            }
            


            //Tile Collision
            foreach (var tile in Tiles)
            {
                foreach (var sprite in sprites)
                {
                    //Touching Up/Down Tile
                    if (sprite.Velocity.Y < 0 && Sprite.IsTouchingTileBottom(sprite, tile))
                    {
                        sprite.Velocity.Y = 0;
                    }
                    if (sprite.Velocity.Y > 0 && Sprite.IsTouchingTileTop(sprite, tile))
                    {
                        isTouchingTile = true;
                        sprite.Velocity.Y = 0;
                        hasJumped = false;
                    }

                    //Touching Left/Right Tile
                    if (sprite.Velocity.X > 0 && Sprite.IsTouchingTileLeft(sprite, tile) ||
                    sprite.Velocity.X < 0 && Sprite.IsTouchingTileRight(sprite, tile))
                    {
                        sprite.Velocity.X = 0;
                    }

                    //Left Boundary (Smaller = Left, Bigger = Right)
                    if (sprite._position.X <= 7)
                    {
                        sprite._position.X = 7;
                    }

                    //Right Boundary (Smaller = Left, Bigger = Right)
                    if (sprite._position.X >= 1550)
                    {
                        sprite._position.X = 1550;
                    }

                    //Up Boundary (Smaller = Up, Bigger = Down)
                    if (sprite._position.Y <= 6)
                    {
                        sprite.Velocity.Y = 0;
                        sprite._position.Y = 7;
                    }

                    //Down Boundary (Smaller = Up, Bigger = Down)
                    if (sprite._position.Y >= 800)
                    {
                        sprite._position.Y = 800;
                        hasJumped = false;
                    }
                }
            }

            //Respawn
            foreach (var sprite in sprites)
            {
                if (sprite.Health <= 0)
                {
                    if (sprite.PlayerNum == 1 && Flag.isPickedUpP1)
                    {
                        Flag.isPickedUpP1 = false;
                    }
                    if (sprite.PlayerNum != 1 && Flag.isPickedUpP2)
                    {
                        Flag.isPickedUpP2 = false;
                    }
                    Respawn(sprite);
                }
            }

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        protected virtual void SetAnimations(List<Sprite> sprites)
        {
            if (Velocity.X > 0 && Velocity.Y == 0)
            {
                _animationManager.Play(_animations["WalkRight"]);
            }
            else if (Velocity.X < 0 && Velocity.Y == 0)
            {
                _animationManager.Play(_animations["WalkLeft"]);
            }
            else if (Velocity.Y != 0 && Velocity.X > 0)
            {
                _animationManager.Play(_animations["FallRight"]);
            }
            else if (Velocity.Y != 0 && Velocity.X < 0)
            {
                _animationManager.Play(_animations["FallLeft"]);
            }
            else if (Velocity.Y != 0 && Velocity.X == 0 && !isTouchingTile)
            {
                if (isFacingLeft)
                {
                    _animationManager.Play(_animations["FallLeft"]);
                }
                else
                {
                    _animationManager.Play(_animations["FallRight"]);
                }
            }
            else if (Velocity.Y == 0 && Velocity.X == 0)
            {
                _animationManager.Stop();
            }

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, 50, 90), Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
        }
        #endregion

        #region Sprite Collision
        protected bool IsTouchingLeft(Sprite sprite)
        {
            return this.Rectangle.Right + this.Velocity.X > sprite.Rectangle.Left + 10 &&
                this.Rectangle.Left < sprite.Rectangle.Left - 10 &&
                this.Rectangle.Bottom > sprite.Rectangle.Top + 9 &&
                this.Rectangle.Top < sprite.Rectangle.Bottom - 9;
        }
        protected bool IsTouchingRight(Sprite sprite)
        {
            return this.Rectangle.Left + this.Velocity.X < sprite.Rectangle.Right - 10 &&
                this.Rectangle.Right > sprite.Rectangle.Right + 10 &&
                this.Rectangle.Bottom > sprite.Rectangle.Top + 9 &&
                this.Rectangle.Top < sprite.Rectangle.Bottom - 9;
        }
        protected bool IsTouchingTop(Sprite sprite)
        {
            return this.Rectangle.Bottom + this.Velocity.Y > sprite.Rectangle.Top + 9 &&
                this.Rectangle.Top < sprite.Rectangle.Top - 9 &&
                this.Rectangle.Right > sprite.Rectangle.Left + 10 &&
                this.Rectangle.Left < sprite.Rectangle.Right - 10;
        }
        protected bool IsTouchingBottom(Sprite sprite)
        {
            return this.Rectangle.Top + this.Velocity.Y < sprite.Rectangle.Bottom - 9 &&
                this.Rectangle.Bottom > sprite.Rectangle.Bottom + 9 &&
                this.Rectangle.Right > sprite.Rectangle.Left + 10 &&
                this.Rectangle.Left < sprite.Rectangle.Right - 10;
        }
        #endregion
        #region Hazard Collision
        protected bool IsTouchingHazardLeft(Sprite sprite, Hazard hazard)
        {
            Rectangle hazardRect = new Rectangle((int) hazard.Position.X, (int) hazard.Position.Y, 50, 90);

            return this.Rectangle.Right + this.Velocity.X > hazardRect.Left + 10 &&
                this.Rectangle.Left < hazardRect.Left - 10 &&
                this.Rectangle.Bottom > hazardRect.Top + 9 &&
                this.Rectangle.Top < hazardRect.Bottom - 9;
        }
        protected bool IsTouchingHazardRight(Sprite sprite, Hazard hazard)
        {
            Rectangle hazardRect = new Rectangle((int)hazard.Position.X, (int)hazard.Position.Y, 50, 90);

            return this.Rectangle.Left + this.Velocity.X < hazardRect.Right - 10 &&
                this.Rectangle.Right > hazardRect.Right + 10 &&
                this.Rectangle.Bottom > hazardRect.Top + 9 &&
                this.Rectangle.Top < hazardRect.Bottom - 9;
        }
        protected bool IsTouchingHazardTop(Sprite sprite, Hazard hazard)
        {
            Rectangle hazardRect = new Rectangle((int)hazard.Position.X, (int)hazard.Position.Y, 50, 90);

            return this.Rectangle.Bottom + this.Velocity.Y > hazardRect.Top + 9 &&
                this.Rectangle.Top < hazardRect.Top - 9 &&
                this.Rectangle.Right > hazardRect.Left + 10 &&
                this.Rectangle.Left < hazardRect.Right - 10;
        }
        protected bool IsTouchingHazardBottom(Sprite sprite, Hazard hazard)
        {
            Rectangle hazardRect = new Rectangle((int)hazard.Position.X, (int)hazard.Position.Y, 50, 90);

            return this.Rectangle.Top + this.Velocity.Y < hazardRect.Bottom - 9 &&
                this.Rectangle.Bottom > hazardRect.Bottom + 9 &&
                this.Rectangle.Right > hazardRect.Left + 10 &&
                this.Rectangle.Left < hazardRect.Right - 10;
        }
        #endregion
        #region Tile Collision
        public static bool IsTouchingTileLeft(Sprite sprite, Rectangle tile)
        {
            return sprite.Rectangle.Right - 7 + sprite.Velocity.X > tile.Left &&
                sprite.Rectangle.Left < tile.Left &&
                sprite.Rectangle.Bottom > tile.Top &&
                sprite.Rectangle.Top + 10 < tile.Bottom;
        }
        public static bool IsTouchingTileRight(Sprite sprite, Rectangle tile)
        {
            return sprite.Rectangle.Left + 7 + sprite.Velocity.X < tile.Right &&
                sprite.Rectangle.Right > tile.Right &&
                sprite.Rectangle.Bottom > tile.Top &&
                sprite.Rectangle.Top + 10 < tile.Bottom;
        }

        public static bool IsTouchingTileBottom(Sprite sprite, Rectangle tile)
        {
            return sprite.Rectangle.Top + 10 + sprite.Velocity.Y < tile.Bottom &&
                sprite.Rectangle.Bottom > tile.Bottom &&
                sprite.Rectangle.Right - 7 > tile.Left &&
                sprite.Rectangle.Left + 7 < tile.Right;
        }
        public static bool IsTouchingTileTop(Sprite sprite, Rectangle tile)
        {
            return sprite.Rectangle.Bottom + sprite.Velocity.Y > tile.Top &&
                sprite.Rectangle.Top < tile.Top &&
                sprite.Rectangle.Right - 7 > tile.Left &&
                sprite.Rectangle.Left + 7 < tile.Right;
        }
        #endregion

        #region Respawn
        public void Respawn(Sprite sprite)
        {
            sprite.Health = 100;
            if (sprite.PlayerNum == 1)
            {
                sprite.Position = new Vector2(1537, 8);
                sprite.Lives--;
            }
            if (sprite.PlayerNum == 2)
            {
                sprite.Position = new Vector2(5, 5);
                sprite.Lives--;
            }
        }
        #endregion
    }
}
