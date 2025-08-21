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
        private float _pulseValue = 0f;
        private const float PulseSpeed = 2f;

        // Confetti properties
        private List<ConfettiParticle> _confettiParticles = new List<ConfettiParticle>();
        private Random _random = new Random();
        private const int ConfettiCount = 150;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");

            // Probeer achtergrond te laden, gebruik placeholder als het mislukt
            try
            {
                _background = content.Load<Texture2D>("ScreenBackgrounds/Victory");
            }
            catch
            {
                
                _background = CreateCelebrationBackground(Game.GraphicsDevice,
                    Game.GraphicsDevice.Viewport.Width,
                    Game.GraphicsDevice.Viewport.Height);
            }

            // Maak pixel texture voor confetti
            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // Initializeer confetti
            InitializeConfetti();
        }

        private void InitializeConfetti()
        {
            var vp = Game.GraphicsDevice.Viewport;
            _confettiParticles.Clear();

            for (int i = 0; i < ConfettiCount; i++)
            {
                _confettiParticles.Add(new ConfettiParticle
                {
                    Position = new Vector2(_random.Next(vp.Width), _random.Next(vp.Height)),
                    Velocity = new Vector2((float)(_random.NextDouble() * 2 - 1), (float)(_random.NextDouble() * 2 - 1)),
                    Color = new Color(
                        (float)_random.NextDouble(),
                        (float)_random.NextDouble(),
                        (float)_random.NextDouble()
                    ),
                    Size = _random.Next(3, 8),
                    Rotation = (float)(_random.NextDouble() * MathHelper.TwoPi),
                    RotationSpeed = (float)(_random.NextDouble() * 0.1f - 0.05f)
                });
            }
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            bool Pressed(Keys k) => kb.IsKeyDown(k) && !_prevKb.IsKeyDown(k);

            if (Pressed(Keys.Enter) || Pressed(Keys.Space) || Pressed(Keys.Escape))
                Screens.ChangeScreen(new StartScreen());

            // Animatiewaarde bijwerken voor knippereffect
            _pulseValue += (float)gameTime.ElapsedGameTime.TotalSeconds * PulseSpeed;

            // Update confetti
            UpdateConfetti(gameTime);

            _prevKb = kb;
        }

        private void UpdateConfetti(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var vp = Game.GraphicsDevice.Viewport;

            for (int i = 0; i < _confettiParticles.Count; i++)
            {
                var particle = _confettiParticles[i];

                // Update positie
                particle.Position += particle.Velocity * 50f * dt;

                // Update rotatie
                particle.Rotation += particle.RotationSpeed;

                // Wrap around scherm
                if (particle.Position.X < 0) particle.Position.X = vp.Width;
                if (particle.Position.X > vp.Width) particle.Position.X = 0;
                if (particle.Position.Y < 0) particle.Position.Y = vp.Height;
                if (particle.Position.Y > vp.Height) particle.Position.Y = 0;

                _confettiParticles[i] = particle;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            var vp = Game.GraphicsDevice.Viewport;

            // Teken achtergrond
            sb.Draw(_background, new Rectangle(0, 0, vp.Width, vp.Height), Color.White);

            // Teken confetti
            DrawConfetti(sb);

            var title = "VICTORY!";
            var sub = "Enter = Terug naar menu";
            var sub2 = "Je hebt alle coins verzameld!";

            var sT = _font.MeasureString(title);
            var sS = _font.MeasureString(sub);
            var sS2 = _font.MeasureString(sub2);

            // Pulserende titel
            float pulseScale = 1.0f + 0.1f * (float)Math.Sin(_pulseValue);
            Color pulseColor = Color.Lerp(Color.Gold, Color.White, 0.5f + 0.5f * (float)Math.Sin(_pulseValue * 2));

            var posT = new Vector2((vp.Width - sT.X) * 0.5f, (vp.Height - sT.Y) * 0.3f);
            var posS = new Vector2((vp.Width - sS.X) * 0.5f, posT.Y + sT.Y + 40);
            var posS2 = new Vector2((vp.Width - sS2.X) * 0.5f, posS.Y + sS.Y + 20);

            // Teken titel met pulse effect
            sb.DrawString(_font, title, posT, pulseColor, 0f, Vector2.Zero, pulseScale, SpriteEffects.None, 0f);
            sb.DrawString(_font, sub2, posS2, Color.LightGreen);
            sb.DrawString(_font, sub, posS, Color.White);
        }

        private void DrawConfetti(SpriteBatch sb)
        {
            foreach (var particle in _confettiParticles)
            {
                sb.Draw(_pixel,
                    new Rectangle((int)particle.Position.X, (int)particle.Position.Y, particle.Size, particle.Size),
                    null, particle.Color, particle.Rotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }

        private Texture2D CreateCelebrationBackground(GraphicsDevice graphicsDevice, int width, int height)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);
            Color[] data = new Color[width * height];

            Color darkBlue = new Color(20, 40, 80);
            Color mediumBlue = new Color(40, 60, 120);

            for (int i = 0; i < data.Length; i++)
            {
                int x = i % width;
                int y = i / width;

                
                float gradientX = (float)x / width;
                float gradientY = (float)y / height;

                Color color = Color.Lerp(
                    Color.Lerp(darkBlue, mediumBlue, gradientY),
                    Color.Lerp(new Color(60, 90, 150), new Color(30, 50, 100), gradientY),
                    gradientX
                );

                data[i] = color;
            }

            texture.SetData(data);
            return texture;
        }

        
        private struct ConfettiParticle
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public Color Color;
            public int Size;
            public float Rotation;
            public float RotationSpeed;
        }
    }
}