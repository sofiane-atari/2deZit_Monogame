using Imenyaan.Entities.Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens
{
    public class GameOverScreen : GameScreen
    {
        private SpriteFont _font;
        private KeyboardState _prevKb;
        private readonly StartScreen.Difficulty _difficulty;
        private readonly ILevelDefinition _level;

        public GameOverScreen(StartScreen.Difficulty difficulty, ILevelDefinition level)
        {
            _difficulty = difficulty;
            _level = level;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            bool Pressed(Keys k) => kb.IsKeyDown(k) && !_prevKb.IsKeyDown(k);

            if (Pressed(Keys.Enter))
            {
                
                Screens.ChangeScreen(new GameplayScreen(_difficulty, _level));
            }
            if (Pressed(Keys.Escape))
            {
                Screens.ChangeScreen(new StartScreen());
            }

            _prevKb = kb;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            var vp = Game.GraphicsDevice.Viewport;
            string title = "GAME OVER";
            string sub = "Enter = Opnieuw  |  Esc = Menu";

            var sizeT = _font.MeasureString(title);
            var sizeS = _font.MeasureString(sub);

            var posT = new Vector2((vp.Width - sizeT.X) * 0.5f, (vp.Height - sizeT.Y) * 0.4f);
            var posS = new Vector2((vp.Width - sizeS.X) * 0.5f, posT.Y + sizeT.Y + 20);

            sb.DrawString(_font, title, posT, Color.Red);
            sb.DrawString(_font, sub, posS, Color.White);
        }

        public override void Unload() { }
    }
}
