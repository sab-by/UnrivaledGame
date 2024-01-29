using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UnrivaledPractise.guibuttons;
using UnrivaledPractise.States;

namespace UnrivaledPractise.States
{
   
    public class MenuState : State
    {
        private List<Components> _components;
        private Texture2D MainMenu, Swords;
        private static int levelselect;
        public static int gettlevel()
        {
            return levelselect;
        }
        public static void  setlevel(int num)
        {
            levelselect = num;

        }
        
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            MainMenu = _content.Load<Texture2D>("Background/MainMenu+");
            Swords = _content.Load<Texture2D>("Background\\SwordsImage");
            var buttonTexture = _content.Load<Texture2D>("guibuttons/Button+");
            var buttonFont = _content.Load<SpriteFont>("guifont/Font");

            var Castleinterior = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(700, 550),
                Text = "Castle Interior",
            };

            Castleinterior.Click += Castleinteriorbutton_Click;

            var Metropolis = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(850, 550),
                Text = "Metropolis",
            };

            Metropolis.Click += Metropolisbutton_Click;

            var Tutorial = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(775, 650),
                Text = "Tutorial",
            };

            Tutorial.Click += Tutorialbutton_Click;
            var Quit = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(900, 800),
                Text = "Quit Game",
            };
            Quit.Click += Quitbutton_Click;
            var Credits = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(650, 800),
                Text = "Credits",
            };
            Credits.Click += Creditsbutton_Click; ;

            _components = new List<Components>()
            {
             Castleinterior,
             Tutorial,
             Metropolis,
                Quit,
                Credits,
               };
        }

        

        private void Castleinteriorbutton_Click(object sender, EventArgs e)
        {
            _game.ChangeMenuState(new GameState(_game, _graphicsDevice, _content));
            setlevel(1);

        }
        private void Metropolisbutton_Click(object sender, EventArgs e)
        {
            _game.ChangeMenuState(new GameState(_game, _graphicsDevice, _content));
            setlevel(2);
        }
        private void Tutorialbutton_Click(object sender, EventArgs e)
        {
            _game.ChangeMenuState(new GameState(_game, _graphicsDevice, _content));
           setlevel(5);
        }


        private void Quitbutton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
        private void Creditsbutton_Click(object sender, EventArgs e)
        {
            _game.ChangeMenuState(new GameState(_game, _graphicsDevice, _content));
            setlevel(3);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MainMenu, new Rectangle(0, 0, 1600, 900), Color.White);

            spriteBatch.Draw(Swords, new Rectangle(790, 440, 100, 100), Color.White);
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);


        }
        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs args)
        {
            _game.Exit();
        }

    }
}