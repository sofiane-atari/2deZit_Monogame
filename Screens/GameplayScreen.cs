using Imenyaan.Core;
using Imenyaan.Entities;
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
        private Hero _hero;

        public GameplayScreen(StartScreen.Difficulty difficulty)
        {
            _difficulty = difficulty;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            _hero = new Hero();
            _hero.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Escape))
                Screens.ChangeScreen(new StartScreen());

            _hero.Update(gameTime, kb);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _hero.Draw(spriteBatch);
            spriteBatch.DrawString(_font, $"Moeilijkheid: {_difficulty}", new Vector2(20, 20), Color.White);
        }
    }
}