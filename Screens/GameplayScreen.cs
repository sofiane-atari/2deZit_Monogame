using Imenyaan.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Imenyaan.Screens
{
    public class GameplayScreen : GameScreen
    {
        private SpriteFont _font;
        private readonly StartScreen.Difficulty _difficulty;
        private KeyboardState _prevKb;

        public GameplayScreen(StartScreen.Difficulty difficulty)
        {
            _difficulty = difficulty;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            bool KeyPressed(Keys k) => kb.IsKeyDown(k) && _prevKb.IsKeyUp(k);

            // Met ESC terug naar het startscherm
            if (KeyPressed(Keys.Escape))
                Screens.ChangeScreen(new StartScreen());

            _prevKb = kb;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var text = $"Gameplay     Moeilijkheid: {_difficulty}. Druk ESC voor menu.";
            var size = _font.MeasureString(text);
            var vp = Game.GraphicsDevice.Viewport;
            var pos = new Vector2((vp.Width - size.X) * 0.5f, (vp.Height - size.Y) * 0.5f);

            spriteBatch.DrawString(_font, text, pos, Color.White);
        }
    }
}