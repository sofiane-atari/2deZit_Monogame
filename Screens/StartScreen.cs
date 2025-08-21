using Imenyaan.Core;
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

        private string _title = "Mijn Game";
        private int _selectedIndex = 0;

        public enum Difficulty { Makkelijk, Normaal, Moeilijk }
        private Difficulty _difficulty = Difficulty.Normaal;

        // Layout
        private Vector2 _titlePos;
        private Vector2 _startPos;
        private Vector2 _difficultyPos;
        private Vector2 _exitPos;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            var vp = Game.GraphicsDevice.Viewport;

            // Eenvoudige centrering
            var titleSize = _font.MeasureString(_title);
            _titlePos = new Vector2((vp.Width - titleSize.X) * 0.5f, 100);

            _startPos = new Vector2(vp.Width * 0.5f, 300);
            _difficultyPos = new Vector2(vp.Width * 0.5f, 360);
            _exitPos = new Vector2(vp.Width * 0.5f, 420);
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            var mouse = Mouse.GetState();

            bool KeyPressed(Keys k) => kb.IsKeyDown(k) && _prevKb.IsKeyUp(k);
            bool MouseClicked() => mouse.LeftButton == ButtonState.Pressed &&
                                   _prevMouse.LeftButton == ButtonState.Released;

            // Navigatie
            if (KeyPressed(Keys.Down))
                _selectedIndex = Math.Min(_selectedIndex + 1, 2);

            if (KeyPressed(Keys.Up))
                _selectedIndex = Math.Max(_selectedIndex - 1, 0);

            // Moeilijkheid aanpassen met ←/→ wanneer die rij geselecteerd is
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
                var mousePos = new Point(mouse.X, mouse.Y);

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
            _prevMouse = mouse;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Titel
            spriteBatch.DrawString(_font, _title, _titlePos, Color.White);

            // Menu-items (gecentreerd)
            DrawMenuItem(spriteBatch, _startPos, "Start", _selectedIndex == 0);
            DrawMenuItem(spriteBatch, _difficultyPos, DifficultyLabel(), _selectedIndex == 1);
            DrawMenuItem(spriteBatch, _exitPos, "Afsluiten", _selectedIndex == 2);
        }

        private void DrawMenuItem(SpriteBatch spriteBatch, Vector2 centerPos, string text, bool selected)
        {
            var size = _font.MeasureString(text);
            var pos = new Vector2(centerPos.X - size.X * 0.5f, centerPos.Y - size.Y * 0.5f);
            spriteBatch.DrawString(_font, text, pos, selected ? Color.Yellow : Color.White);
        }

        private Rectangle GetItemRect(Vector2 centerPos, string text)
        {
            var size = _font.MeasureString(text);
            var pos = new Vector2(centerPos.X - size.X * 0.5f, centerPos.Y - size.Y * 0.5f);
            return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        private string DifficultyLabel() => $"Moeilijkheid: {_difficulty}";

        private void NextDifficulty() =>
            _difficulty = _difficulty == Difficulty.Moeilijk ? Difficulty.Makkelijk : _difficulty + 1;

        private void PrevDifficulty() =>
            _difficulty = _difficulty == Difficulty.Makkelijk ? Difficulty.Moeilijk : _difficulty - 1;

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
