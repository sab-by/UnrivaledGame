using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnrivaledPractise.States;
using UnrivaledPractise.Gui;
using System.Diagnostics;

namespace UnrivaledPractise
{
    public class Game1 : Game
    {
        #region Declarations
        //GUI
        //GUI
        Texture2D playerGui, winscreen, Endp1, Endp2, Creditscreen;
        public enum STATES { Title, Play, Win, Credits };
        STATES thistate = STATES.Title;
        float seconds = 0.0f;
        float windscreentimer = 5.0f;
        int highestp1 = 0;
        int highestp2 = 0;
        float creditimer = 25f;
        float gametimer = 90f;
        float gametimer2 = 90f;
        bool p1Win = false;
        bool p2Win = false;
        public static bool titlelock = false;


        //states
        private State _currentMenuState;
        private State _nextMenuState;
        private State _lastState;
        public void ChangeMenuState(State state)
        {
            _nextMenuState = state;
        }



        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Texture2D LevelOne, Frame, TileSheet, RedPlayer, BluePlayer, bulletTextureLeft, bulletTextureRight, flag, MainMenu, LevelTwo, HazardRight, HazardLeft, Tut01, Tut02, Tut03R, Tut03B, Swords;
        private SpriteFont HealthFont;
        private List<Sprite> Sprites;
        private List<Hazard> Hazards;

        BulletManager Bullets = new BulletManager();
        private List<Rectangle> tilesLv1 = Tile.GetTileLv1List();
        private List<Rectangle> tilesLv2 = Tile.GetTileLv2List();
        private List<Rectangle> tilesTut = Tile.GetTileTutList();
        private List<Rectangle> p1tiles = Tile.GetP1TilesList();
        private List<Rectangle> p2tiles = Tile.GetP2TilesList();

        private Rectangle Tut01Rect = new Rectangle (0, 0, 1600, 450);
        private Rectangle Tut02Rect = new Rectangle(0, 450, 1600, 450);
        #endregion

        #region Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        #endregion

        #region Initialize
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            IsMouseVisible = true;
            graphics.ApplyChanges();
            base.Initialize();
        }
        #endregion

        #region Load Content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //States
            MainMenu = Content.Load<Texture2D>("Background/MainMenu+");
            winscreen = Content.Load<Texture2D>("Background/Endscreen+");
            Creditscreen = Content.Load<Texture2D>("Background/Game_Credits");
            _currentMenuState = new MenuState(this, graphics.GraphicsDevice, Content);
            Endp1 = Content.Load<Texture2D>("Sprites/ClassicBlueSprite");
            Endp2 = Content.Load<Texture2D>("Sprites/ClassicRedSprite");

            //Tutorial
            Tut01 = Content.Load<Texture2D>("Background\\Tut01");
            Tut02 = Content.Load<Texture2D>("Background\\Tut02");
            Tut03R = Content.Load<Texture2D>("Background\\Tut03Red");
            Tut03B = Content.Load<Texture2D>("Background\\Tut03Blue");

            //Levels & Tiles
            TileSheet = Content.Load<Texture2D>("Sheets\\TileSheet20x20");
            LevelOne = Content.Load<Texture2D>("Background\\Lv1");
            LevelTwo = Content.Load<Texture2D>("Background\\Lv2");
            Frame = Content.Load<Texture2D>("Background\\Frame");

            //Players
            RedPlayer = Content.Load<Texture2D>("Sprites\\RedSprite");
            BluePlayer = Content.Load<Texture2D>("Sprites\\BlueSprite");

            // Lives
            playerGui = Content.Load<Texture2D>("guibuttons/heart");

            //Hazard
            HazardRight = Content.Load<Texture2D>("Sprites\\Hazard");
            HazardLeft = Content.Load<Texture2D>("Sprites\\Hazard2");

            //Fonts
            HealthFont = Content.Load<SpriteFont>("Fonts\\Health");

            //Bullet
            bulletTextureRight = Content.Load<Texture2D>("Bullet\\BulletRight");
            bulletTextureLeft = Content.Load<Texture2D>("Bullet\\BulletLeft");

            //Flag
            flag = Content.Load<Texture2D>("Sprites\\Flag");

            var P1animations = new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(Content.Load<Texture2D>("Sheets\\P1WalkRightSheet"), 6, true) },
                { "WalkLeft", new Animation(Content.Load<Texture2D>("Sheets\\P1WalkLeftSheet"), 6, true) },
                { "FallLeft", new Animation(Content.Load<Texture2D>("Sheets\\P1FallLeftSheet"), 2, true) },
                { "FallRight", new Animation(Content.Load<Texture2D>("Sheets\\P1FallRightSheet"), 2, true) },
            };
            var P2animations = new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(Content.Load<Texture2D>("Sheets\\P2WalkRightSheet"), 6, true) },
                { "WalkLeft", new Animation(Content.Load<Texture2D>("Sheets\\P2WalkLeftSheet"), 6, true) },
                { "FallLeft", new Animation(Content.Load<Texture2D>("Sheets\\P2FallLeftSheet"), 2, true) },
                { "FallRight", new Animation(Content.Load<Texture2D>("Sheets\\P2FallRightSheet"), 2, true) },
            };

            Sprites = new List<Sprite>()
            {
                new Sprite(P1animations)
                {
                    PlayerNum = 1,
                    Position = new Vector2(1537, 10),
                    HPPosition = new Vector2(1340, 10),
                    isFacingLeft = true,
                    Speed = 10f,
                    Health = 100,
                    Lives = 3,
                    LivesPosition = new Vector2 (1000, 500),
                    Input = new Input()
                    {
                        Up = Keys.Up,
                        Down = Keys.Down,
                        Left = Keys.Left,
                        Right = Keys.Right,
                        Shoot = Keys.End
                    }
                },

                new Sprite(P2animations)
                {
                    PlayerNum = 2,
                    Position = new Vector2(10, 10),
                    HPPosition = new Vector2(12, 10),
                    Speed = 3f,
                    Health = 100,
                    Lives = 3,
                    LivesPosition = new Vector2 (400, 500),
                    Input = new Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Shoot = Keys.B
                    }
                }
            };

            Hazards = new List<Hazard>()
            {
                //Level 1 & 2 Hazard
                new Hazard(300, HazardRight, new Vector2(10, 290))
                {
                    hazardNum = 1,
                    isADummy = false
                },
                //Tutorial Hazards
                new Hazard(200, HazardLeft, new Vector2(470, 790))
                {
                    hazardNum = 1,
                    isADummy = true
                },
                new Hazard(200, HazardLeft, new Vector2(530, 790))
                {
                    hazardNum = 1,
                    isADummy = true
                },
                new Hazard(200, HazardLeft, new Vector2(590, 790))
                {
                    hazardNum = 1,
                    isADummy = true
                },
                new Hazard(200, HazardRight, new Vector2(1010, 790))
                {
                    hazardNum = 1,
                    isADummy = true
                },
                new Hazard(200, HazardRight, new Vector2(1070, 790))
                {
                    hazardNum = 1,
                    isADummy = true
                },
                new Hazard(200, HazardRight, new Vector2(1130, 790))
                {
                    hazardNum = 1,
                    isADummy = true
                },
            };

            Bullets.Initialize(bulletTextureLeft, bulletTextureRight, GraphicsDevice);
            Flag.Initialize(flag);
            Tile.Initialize(TileSheet);
        }
        #endregion

        #region Update and Draw
        protected override void Update(GameTime gameTime)
        {
            if (_nextMenuState != null)
            {
                _lastState = _currentMenuState;
                _currentMenuState = _nextMenuState;

                _nextMenuState = null;
            }


            _currentMenuState.Update(gameTime);
            _currentMenuState.PostUpdate(gameTime);


            switch (thistate)
            {
                case STATES.Title:
                    titlelock = false;
                    foreach (var sprite in Sprites)
                    { sprite.Speed = 0; }

                    if (MenuState.gettlevel() == 1)
                    {
                        thistate = STATES.Play;


                    }
                    if (MenuState.gettlevel() == 2)
                    {
                        thistate = STATES.Play;

                    }
                    if (MenuState.gettlevel() == 5)
                    {
                        thistate = STATES.Play;

                    }
                    if (MenuState.gettlevel() == 3)
                    {

                        thistate = STATES.Credits;
                        MenuState.setlevel(9);
                    }

                    break;

                case STATES.Play:
                    gametimer2 -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        thistate = STATES.Title;
                        MenuState.setlevel(9);
                        _currentMenuState = _lastState;
                        gametimer2 = 90f;

                        reinitialize(Sprites);
                    }

                    titlelock = true;
                    foreach (var sprite in Sprites)
                    { sprite.Speed = 3; }
                    seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Debug.WriteLine(seconds);
                    if (seconds >= gametimer)
                    {
                        thistate = STATES.Win;
                        seconds = 0f;
                        gametimer2 = 90f;
                        foreach (var sprite in Sprites)
                        {

                            if (sprite.PlayerNum == 1)
                            {
                                highestp1 = sprite.Lives;

                            }
                            if (sprite.PlayerNum != 1)
                            {
                                highestp2 = sprite.Lives;

                            }

                        }
                        if (highestp1 > highestp2)
                        {
                            p2Win = true;
                        }
                        else if (highestp2 > highestp1)
                        {
                            p1Win = true;
                        }
                    }
                    foreach (var sprite in Sprites)
                    {

                        if (sprite.PlayerNum == 1)
                        {
                            if (sprite.Lives <= 0 || Events.FlagCapP1)
                            {
                                thistate = STATES.Win;
                                p1Win = true;
                                seconds = 0f;
                                gametimer2 = 90f;
                            }
                        }
                        if (sprite.PlayerNum != 1)
                        {
                            if (sprite.Lives <= 0 || Events.FlagCapP2)
                            {
                                p2Win = true;
                                thistate = STATES.Win;
                                seconds = 0f;
                                gametimer2 = 90f;
                            }
                        }
                    }

                    break;

                case STATES.Win:
                    foreach (var sprite in Sprites)
                    { sprite.Speed = 0; }
                    titlelock = false;
                    
                    MenuState.setlevel(9);
                    seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Debug.WriteLine(seconds);
                    if (seconds >= windscreentimer)
                    {
                        reinitialize(Sprites);
                        thistate = STATES.Title;
                        
                        if (MenuState.gettlevel() == 9)
                        {
                            _currentMenuState = _lastState;
                            _currentMenuState.Update(gameTime);
                            MenuState.setlevel(8);
                        }
                        p1Win = false;
                        p2Win = false;
                    }
                    break;
                case STATES.Credits:
                    seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    titlelock = false;
                    Debug.WriteLine(seconds);
                    if (seconds >= creditimer)
                    {
                        seconds = 0f;
                        thistate = STATES.Title;

                        if (MenuState.gettlevel() == 9)
                        {
                            _currentMenuState = _lastState;
                            _currentMenuState.Update(gameTime);
                            MenuState.setlevel(8);
                        }
                    }

                    break;

            }

            // TODO: Add your update logic here
            //Update each sprite
            foreach (var sprite in Sprites)
            {
                if (MenuState.gettlevel() == 1)
                {
                    sprite.Update(gameTime, tilesLv1, p1tiles, p2tiles, Sprites, Hazards);
                }
                if (MenuState.gettlevel() == 2)
                {
                    sprite.Update(gameTime, tilesLv2, p1tiles, p2tiles, Sprites, Hazards);
                }
                if (MenuState.gettlevel() == 5)
                {
                    sprite.Update(gameTime, tilesTut, p1tiles, p2tiles, Sprites, Hazards);
                }
            }
            foreach (var hazard in Hazards)
            {

                if (MenuState.gettlevel() == 1)
                {
                    if (!hazard.isADummy)
                    {
                        hazard.Position.Y = 290;
                        hazard.Update(gameTime, Hazards, Sprites);
                        hazard.canCollide = true;
                    }
                    else
                    {
                        hazard.canCollide = false;
                    }   
                }
                if (MenuState.gettlevel() == 2)
                {
                    if (!hazard.isADummy)
                    {
                        hazard.Position.Y = 350;
                        hazard.Update(gameTime, Hazards, Sprites);
                        hazard.canCollide = true;
                    }
                    else
                    {
                        hazard.canCollide = false;
                    }
                }
                if (MenuState.gettlevel() == 5)
                {
                    if (!hazard.isADummy)
                    {
                        hazard.canCollide = false;
                    }
                    else
                    {
                        hazard.canCollide = true;
                    }
                }
            }
            Bullets.UpdateManagerBullet(gameTime, Sprites, Hazards);
            Tile.Update(gameTime, Sprites);
            Flag.Update(gameTime, Sprites);
            Events.Update(gameTime, Sprites, p1tiles, p2tiles);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (thistate == STATES.Title)
            {
                _currentMenuState.Draw(gameTime, spriteBatch);
            }

            if (thistate == STATES.Play)
            {
                if (MenuState.gettlevel() == 1)
                {

                    spriteBatch.Draw(LevelOne, new Rectangle(0, 0, 1600, 900), Color.White);
                    Tile.DrawLevel1(spriteBatch);
                    Flag.Draw(spriteBatch);
                    playergui(Sprites);

                    foreach (var hazard in Hazards)
                    {
                        if (!hazard.isADummy)
                        hazard.Draw(spriteBatch);
                    }
                    spriteBatch.DrawString(HealthFont, "Time " + gametimer2, new Vector2(650, 20), Color.Red);

                }
                else if (MenuState.gettlevel() == 2)
                {
                    spriteBatch.Draw(LevelTwo, new Rectangle(0, 0, 1600, 900), Color.White);
                    Tile.DrawLevel2(spriteBatch);
                    Flag.Draw(spriteBatch);
                    playergui(Sprites);

                    foreach (var hazard in Hazards)
                    {
                        if (!hazard.isADummy)
                        hazard.Draw(spriteBatch);
                    }

                    spriteBatch.DrawString(HealthFont, "Time " + gametimer2, new Vector2(650, 20), Color.Red);
                }
                else if (MenuState.gettlevel() == 5)
                {
                    
                    spriteBatch.Draw(LevelOne, new Rectangle(0, 0, 1600, 900), Color.White);
                    Tile.DrawLevelTut(spriteBatch);
                    Flag.Draw(spriteBatch);
                    playergui(Sprites);
                    foreach (var hazard in Hazards)
                    {
                        if (hazard.isADummy)
                        hazard.Draw(spriteBatch);
                    }
                    foreach (var sprite in Sprites)
                    {
                        if (sprite.Rectangle.Intersects(Tut01Rect) && !sprite.Rectangle.Intersects(Tut02Rect) && !Flag.isPickedUpP1 && !Flag.isPickedUpP2)
                        {
                            spriteBatch.Draw(Tut01, new Rectangle(500, 300, 600, 400), Color.White);
                        }
                        if (sprite.Rectangle.Intersects(Tut02Rect) && sprite.Rectangle.Intersects(Tut02Rect) && !Flag.isPickedUpP1 && !Flag.isPickedUpP2)
                        {
                            spriteBatch.Draw(Tut02, new Rectangle(500, 300, 600, 400), Color.White);
                        }
                        if (Flag.isPickedUpP1 && !Flag.isPickedUpP2)
                        {
                            spriteBatch.Draw(Tut03R, new Rectangle(500, 300, 600, 400), Color.White);
                        }
                        if (Flag.isPickedUpP2 && !Flag.isPickedUpP1)
                        {
                            spriteBatch.Draw(Tut03B, new Rectangle(500, 300, 600, 400), Color.White);
                        }
                    }
                }

                Bullets.DrawBullets(spriteBatch);
            }
            if (thistate == STATES.Win)
            {
                spriteBatch.Draw(winscreen, new Rectangle(0,0,1600,900), Color.White);
                if (p1Win== true)
                {
                    spriteBatch.Draw(Endp1, new Rectangle(500, 150, 150, 250), Color.White);
                    spriteBatch.Draw(Endp2, new Rectangle(940, 294, 150, 250), Color.White);
                }
                if (p2Win == true)
                {
                    spriteBatch.Draw(Endp2, new Rectangle(500, 150, 150, 250), Color.White);
                    spriteBatch.Draw(Endp1, new Rectangle(940, 294, 150, 250), Color.White); ;
                }

            }
            
            if (thistate == STATES.Credits)
            {
                spriteBatch.Draw(Creditscreen, new Rectangle(0, 0, 1600, 900), Color.White);
            }
            spriteBatch.Draw(Frame, new Rectangle(0, 0, 1600, 900), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion
        public void reinitialize(List<Sprite> sprites)
        {
            BulletManager.Bullets.Clear();
            foreach (var sprite in sprites)
            {
                {
                    seconds = 0f; ;
                    Events.FlagCapP1 = false;
                    Events.FlagCapP2 = false;
                    Flag.isPickedUpP1 = false;
                    Flag.isPickedUpP2 = false;
                    sprite.Lives = 3;
                    sprite.Health = 100;
                    Flag.Position = new Vector2(800, 830);
                    if (sprite.PlayerNum != 1)
                    {
                        sprite.Position = new Vector2(10, 10);
                    }
                    if (sprite.PlayerNum == 1)
                    {
                        sprite.Position = new Vector2(1537, 10);
                    }
                }

            }
            foreach (var hazard in Hazards)
            {
                hazard.Health = 100;
            }
        }

        public void playergui(List<Sprite> sprites)
        {
            foreach (var sprite in Sprites)
            {
                if (sprite.PlayerNum == 1)
                {

                    if (sprite.Health > 0)
                    {
                        sprite.Draw(spriteBatch);
                        spriteBatch.DrawString(HealthFont, "HP: " + sprite.Health, new Vector2(sprite.HPPosition.X, sprite.HPPosition.Y), Color.Red);
                    }
                    for (int i = 1; i <= sprite.Lives; i++)
                    {
                        spriteBatch.Draw(playerGui, new Rectangle((int)sprite.LivesPosition.X + (i * 50), 30, 30, 30), Color.White);
                    }
                }
                if (sprite.PlayerNum != 1)
                {
                    if (sprite.Health > 0)
                    {
                        sprite.Draw(spriteBatch);
                        spriteBatch.DrawString(HealthFont, "HP: " + sprite.Health, new Vector2(sprite.HPPosition.X, sprite.HPPosition.Y), Color.Blue);
                    }
                    for (int i = 1; i <= sprite.Lives; i++)
                    {
                        spriteBatch.Draw(playerGui, new Rectangle((int)sprite.LivesPosition.X + (i * 50), 30, 30, 30), Color.White);
                    }
                }
            }
        }


    }



    #region Program Class
    public static class Program
        {
            static void Main()
            {
                using (var game = new Game1())
                    game.Run();
            }
        }
    #endregion
}