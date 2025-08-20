using Imenyaan.Core;
using Imenyaan.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Imenyaan.Screens
{
    public class GameplayScreen : GameScreen
    {
        private SpriteFont _font;
        private readonly StartScreen.Difficulty _difficulty;
        private KeyboardState _prevKb;
        private Hero _hero;

        private Texture2D _background;
        private List<Rectangle> _obstacles;
        private Texture2D _pixel;

        public GameplayScreen(StartScreen.Difficulty difficulty)
        {
            _difficulty = difficulty;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");

            _background = content.Load<Texture2D>("Sprites/Background");
            

            _obstacles = new List<Rectangle>
                {
                    new Rectangle(150, 120, 200, 40),  // obstakels
                    new Rectangle(500, 250, 60, 180),  // 
                    new Rectangle(300, 420, 260, 40),  // 

                    new Rectangle(900, 500, 80, 80),    // doos 4
                    new Rectangle(1100, 550, 120, 60),  // doos 5
                };

            // 3) 1×1 pixel texture voor debug (rechthoeken tekenen)
            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // 4) Hero
            _hero = new Hero();
            _hero.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape))
                Screens.ChangeScreen(new StartScreen());

            _hero.UpdateWithCollisionAgainstBoxes(gameTime, kb, _obstacles);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // 1) Achtergrond (volledig scherm vullen)
            var vp = Game.GraphicsDevice.Viewport;
            var dst = new Rectangle(0, 0, vp.Width, vp.Height);
            spriteBatch.Draw(_background, dst, Color.White);

            // 2) Obstakels (optioneel zichtbaar voor debug)
            foreach (var r in _obstacles)
                spriteBatch.Draw(_pixel, r, Color.Black * 0.25f);

            _hero.Draw(spriteBatch);

            spriteBatch.DrawString(_font, "ESC = menu", new Vector2(20, 20), Color.White);
        }
    }
}