using Imenyaan.Entities.Definitions;
using Imenyaan.Screens.ScreenRefactorHelpers;
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

        private float _pulse, _fade;
        private const float PulseSpeed = 2f, FadeSpeed = 1.5f;

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
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pulse += dt * PulseSpeed;
            _fade = MathHelper.Clamp(_fade + dt * FadeSpeed, 0f, 1f);

            var kb = Keyboard.GetState();
            bool Pressed(Keys k) => kb.IsKeyDown(k) && !_prevKb.IsKeyDown(k);

            if (Pressed(Keys.Enter))
            {
                Screens.ChangeScreen(new GameplayScreen(_difficulty, _level));
                return;
            }
            if (Pressed(Keys.Escape))
            {
                Screens.ChangeScreen(new StartScreen());
                return;
            }

            _prevKb = kb;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            var vp = Game.GraphicsDevice.Viewport;

            // Achtergrondkleur per moeilijkheid
            var bg = _difficulty == StartScreen.Difficulty.Moeilijk ? new Color(80, 0, 0) : new Color(40, 0, 0);
            sb.GraphicsDevice.Clear(bg * 0.9f);

            // Teksten
            string title = "GAME OVER";
            string sub = "Enter = Opnieuw  |  Esc = Menu";

            float titleScale = 1.0f + 0.1f * (float)System.Math.Sin(_pulse * 3f);
            var titleColor = Color.Lerp(Color.Red, new Color(180, 0, 0), UiAnim.Osc(_pulse));

            TextRenderer.DrawCentered(sb, _font, title,
                new Vector2(vp.Width * 0.5f, vp.Height * 0.4f),
                titleScale, titleColor, UiTheme.Shadow);

            var subColor = Color.Lerp(Color.Transparent, UiTheme.Text, _fade);
            TextRenderer.DrawCentered(sb, _font, sub,
                new Vector2(vp.Width * 0.5f, vp.Height * 0.6f),
                1f, subColor);
        }
    }
}
