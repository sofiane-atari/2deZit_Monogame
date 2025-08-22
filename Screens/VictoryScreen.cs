using Imenyaan.Screens.ScreenRefactorHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Imenyaan.Screens
{
    public class VictoryScreen : GameScreen
    {
        private SpriteFont _font;
        private KeyboardState _prevKb;

        private Texture2D _background;  
        private Texture2D _pixel;
        private ParticleSystem _confetti;

        private float _pulse;
        private const float PulseSpeed = 2f;
        private const int ConfettiCount = 150;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");

            // achtergrond (best effort)
            try { _background = content.Load<Texture2D>("ScreenBackgrounds/Victory"); }
            catch { _background = null; }

            // pixel + confetti
            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _confetti = new ParticleSystem(_pixel, new WrapBehavior());
            var vp = Game.GraphicsDevice.Viewport;
            _confetti.SpawnConfetti(ConfettiCount, new Rectangle(0, 0, vp.Width, vp.Height));
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            bool Pressed(Keys k) => kb.IsKeyDown(k) && !_prevKb.IsKeyDown(k);
            if (Pressed(Keys.Enter) || Pressed(Keys.Space) || Pressed(Keys.Escape))
            {
                Screens.ChangeScreen(new StartScreen());
                return;
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pulse += dt * PulseSpeed;

            var vp = Game.GraphicsDevice.Viewport;
            _confetti.Update(dt, new Rectangle(0, 0, vp.Width, vp.Height));

            _prevKb = kb;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            var vp = Game.GraphicsDevice.Viewport;

            if (_background != null)
                sb.Draw(_background, new Rectangle(0, 0, vp.Width, vp.Height), Color.White);
            else
                sb.GraphicsDevice.Clear(UiTheme.BgDark);

            _confetti.Draw(sb);

            string title = "VICTORY!";
            string sub1 = "Je hebt alle coins verzameld!";
            string sub2 = "Enter = Terug naar menu";

            float titleScale = UiTheme.TitleBaseScale * UiAnim.Pulse(_pulse, UiTheme.TitlePulseAmp);
            var titleColor = Color.Lerp(UiTheme.Title1, UiTheme.Title2, UiAnim.Osc(_pulse));

            TextRenderer.DrawCentered(sb, _font, title,
                new Vector2(vp.Width * 0.5f, vp.Height * 0.35f),
                titleScale, titleColor, UiTheme.Shadow);

            TextRenderer.DrawCentered(sb, _font, sub1,
                new Vector2(vp.Width * 0.5f, vp.Height * 0.35f + 100),
                1f, Color.LightGreen);

            TextRenderer.DrawCentered(sb, _font, sub2,
                new Vector2(vp.Width * 0.5f, vp.Height * 0.35f + 160),
                1f, UiTheme.Text);
        }

        public override void Unload()
        {
            _pixel?.Dispose();
        }
    }
}