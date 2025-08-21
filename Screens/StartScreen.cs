using Imenyaan.Entities.Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Imenyaan.Screens
{
    public class StartScreen : GameScreen
    {
        private SpriteFont _font;
        private KeyboardState _prevKb;
        private MouseState _prevMouse;
        private MouseState _currentMouse;

        private string _title = "Survival";
        private int _selectedIndex = 0;
        private float _inputBlock = 0.15f;
        private float _pulseValue = 0f;
        private const float PulseSpeed = 3f;

        public enum Difficulty { Makkelijk, Moeilijk }
        private Difficulty _difficulty = Difficulty.Makkelijk;

        // Layout
        private Vector2 _titlePos;
        private Vector2 _startPos;
        private Vector2 _difficultyPos;
        private Vector2 _exitPos;

        // Animation
        private float _hoverTimer = 0f;
        private const float HoverSpeed = 4f;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            var vp = Game.GraphicsDevice.Viewport;

            var titleSize = _font.MeasureString(_title);
            _titlePos = new Vector2((vp.Width - titleSize.X) * 0.5f, 120);

            _startPos = new Vector2(vp.Width * 0.5f, 280);
            _difficultyPos = new Vector2(vp.Width * 0.5f, 340);
            _exitPos = new Vector2(vp.Width * 0.5f, 400);
        }

        public override void Update(GameTime gameTime)
        {
            _currentMouse = Mouse.GetState();

            if (_inputBlock > 0f)
            {
                _inputBlock -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                _prevKb = Keyboard.GetState();
                _prevMouse = _currentMouse;
                return;
            }

            var kb = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update animations
            _pulseValue += dt * PulseSpeed;
            _hoverTimer += dt * HoverSpeed;

            bool KeyPressed(Keys k) => kb.IsKeyDown(k) && _prevKb.IsKeyUp(k);
            bool MouseClicked() => _currentMouse.LeftButton == ButtonState.Pressed &&
                                   _prevMouse.LeftButton == ButtonState.Released;

            // Navigatie
            if (KeyPressed(Keys.Down))
            {
                _selectedIndex = Math.Min(_selectedIndex + 1, 2);
            }

            if (KeyPressed(Keys.Up))
            {
                _selectedIndex = Math.Max(_selectedIndex - 1, 0);
            }

            // Moeilijkheid aanpassen met ←/→ 
            if (_selectedIndex == 1)
            {
                if (KeyPressed(Keys.Left)) PrevDifficulty();
                if (KeyPressed(Keys.Right)) NextDifficulty();
            }

            // Selecteren met Enter
            if (KeyPressed(Keys.Enter)) ActivateSelected();

            // Muisklikken op items
            if (MouseClicked())
            {
                var mousePos = new Point(_currentMouse.X, _currentMouse.Y);

                if (GetItemRect(_startPos, "Start").Contains(mousePos))
                {
                    _selectedIndex = 0;
                    ActivateSelected();
                }
                else if (GetItemRect(_difficultyPos, DifficultyLabel()).Contains(mousePos))
                {
                    _selectedIndex = 1;
                    NextDifficulty();
                }
                else if (GetItemRect(_exitPos, "Afsluiten").Contains(mousePos))
                {
                    _selectedIndex = 2;
                    ActivateSelected();
                }
            }

            _prevKb = kb;
            _prevMouse = _currentMouse;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var vp = Game.GraphicsDevice.Viewport;

            // Donkere achtergrond
            spriteBatch.GraphicsDevice.Clear(new Color(20, 30, 50));

            // Titel met pulse effect en schaduw
            float titleScale = 1.5f + 0.1f * (float)Math.Sin(_pulseValue);
            Color titleColor = Color.Lerp(Color.Gold, Color.Orange, 0.5f + 0.5f * (float)Math.Sin(_pulseValue * 0.5f));

            Vector2 titleOrigin = _font.MeasureString(_title) * 0.5f;
            Vector2 titleShadowPos = _titlePos + new Vector2(3, 3);

            // Schaduw
            spriteBatch.DrawString(_font, _title, titleShadowPos, Color.Black * 0.5f, 0f, titleOrigin, titleScale, SpriteEffects.None, 0f);
            // Hoofdtekst
            spriteBatch.DrawString(_font, _title, _titlePos, titleColor, 0f, titleOrigin, titleScale, SpriteEffects.None, 0f);

            // Menu-items met hover effect
            DrawMenuItem(spriteBatch, _startPos, "Start", _selectedIndex == 0);
            DrawMenuItem(spriteBatch, _difficultyPos, DifficultyLabel(), _selectedIndex == 1);
            DrawMenuItem(spriteBatch, _exitPos, "Afsluiten", _selectedIndex == 2);

            // Eenvoudige cursor
            DrawSimpleCursor(spriteBatch);
        }

        private void DrawMenuItem(SpriteBatch spriteBatch, Vector2 centerPos, string text, bool selected)
        {
            var size = _font.MeasureString(text);
            var pos = new Vector2(centerPos.X - size.X * 0.5f, centerPos.Y - size.Y * 0.5f);

            // Hover effect voor muis
            var mousePos = new Point(_currentMouse.X, _currentMouse.Y);
            var itemRect = GetItemRect(centerPos, text);
            bool isHovered = itemRect.Contains(mousePos);

            // Kleur en schaal bepalen
            Color color;
            float scale = 1f;

            if (selected)
            {
                // Geselecteerd item
                float pulse = 0.1f * (float)Math.Sin(_pulseValue * 4f);
                color = Color.Lerp(Color.Yellow, Color.Orange, 0.5f + pulse);
                scale = 1.1f + pulse;
            }
            else if (isHovered)
            {
                // Hover effect
                float hover = 0.5f + 0.5f * (float)Math.Sin(_hoverTimer);
                color = Color.Lerp(Color.White, Color.LightBlue, hover);
                scale = 1.05f;
            }
            else
            {
                // Normaal item
                color = Color.White;
            }

            // Tekenen met schaal
            Vector2 origin = size * 0.5f;
            Vector2 drawPos = centerPos;

            spriteBatch.DrawString(_font, text, drawPos, color, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        private void DrawSimpleCursor(SpriteBatch spriteBatch)
        {
            // Eenvoudige witte cursor
            spriteBatch.DrawString(_font, ">", new Vector2(_currentMouse.X - 15, _currentMouse.Y - 5), Color.White);
            spriteBatch.DrawString(_font, "<", new Vector2(_currentMouse.X + 5, _currentMouse.Y - 5), Color.White);
        }

        private Rectangle GetItemRect(Vector2 centerPos, string text)
        {
            var size = _font.MeasureString(text);
            var pos = new Vector2(centerPos.X - size.X * 0.5f, centerPos.Y - size.Y * 0.5f);
            return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        private string DifficultyLabel() => $"Moeilijkheid: {_difficulty}";

        private void NextDifficulty() =>
            _difficulty = _difficulty == Difficulty.Moeilijk ? Difficulty.Makkelijk : Difficulty.Moeilijk;

        private void PrevDifficulty() =>
            _difficulty = _difficulty == Difficulty.Makkelijk ? Difficulty.Moeilijk : Difficulty.Makkelijk;

        private void ActivateSelected()
        {
            switch (_selectedIndex)
            {
                case 0: // Start
                    Screens.ChangeScreen(new GameplayScreen(_difficulty, new Level1Definition()));
                    break;
                case 1: // Moeilijkheid
                    NextDifficulty();
                    break;
                case 2: // Afsluiten
                    Game.Exit();
                    break;
            }
        }
    }
}