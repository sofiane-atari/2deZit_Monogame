using Imenyaan.Entities.Definitions;
using Imenyaan.Screens.ScreenRefactorHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Imenyaan.Screens
{
    public class StartScreen : GameScreen
    {
        private SpriteFont _font;
        private KeyboardState _prevKb;
        private MouseState _prevMouse, _mouse;

        private float _pulse, _hover;
        private const float PulseSpeed = 3f, HoverSpeed = 4f;

        private string _title = "Survival";
        public enum Difficulty { Makkelijk, Moeilijk }
        private Difficulty _difficulty = Difficulty.Makkelijk;

        private readonly List<MenuItem> _items = new();
        private int _selected = 0;

        // posities
        private Vector2 _titleCenter, _startCenter, _diffCenter, _exitCenter;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            var vp = Game.GraphicsDevice.Viewport;

            _titleCenter = new Vector2(vp.Width * 0.5f, 120);
            _startCenter = new Vector2(vp.Width * 0.5f, 280);
            _diffCenter = new Vector2(vp.Width * 0.5f, 340);
            _exitCenter = new Vector2(vp.Width * 0.5f, 400);

            _items.Clear();
            _items.Add(new MenuItem("Start",
                () => Screens.ChangeScreen(new GameplayScreen(_difficulty, new Level1Definition())),
                _startCenter));

            _items.Add(new MenuItem(DifficultyLabel(),
                () => { ToggleDifficulty(); _items[1].Text = DifficultyLabel(); },
                _diffCenter));

            _items.Add(new MenuItem("Afsluiten", () => Game.Exit(), _exitCenter));
        }

        public override void Update(GameTime gameTime)
        {
            _mouse = Mouse.GetState();
            var kb = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _pulse += dt * PulseSpeed;
            _hover += dt * HoverSpeed;

            bool Pressed(Keys k) => kb.IsKeyDown(k) && _prevKb.IsKeyUp(k);
            bool Clicked() => _mouse.LeftButton == ButtonState.Pressed &&
                              _prevMouse.LeftButton == ButtonState.Released;

            if (Pressed(Keys.Down)) _selected = Math.Min(_selected + 1, _items.Count - 1);
            if (Pressed(Keys.Up)) _selected = Math.Max(_selected - 1, 0);
            if (Pressed(Keys.Enter)) _items[_selected].OnActivate();

            // muis
            for (int i = 0; i < _items.Count; i++)
            {
                if (IsMouseOver(_items[i]))
                {
                    _selected = i;
                    if (Clicked()) _items[i].OnActivate();
                }
            }

            _prevKb = kb;
            _prevMouse = _mouse;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            var vp = Game.GraphicsDevice.Viewport;
            sb.GraphicsDevice.Clear(UiTheme.BgDark);

            float tScale = UiTheme.TitleBaseScale * UiAnim.Pulse(_pulse, UiTheme.TitlePulseAmp);
            var tColor = Color.Lerp(UiTheme.Title1, UiTheme.Title2, UiAnim.Osc(_pulse * 0.5f));
            TextRenderer.DrawCentered(sb, _font, _title, _titleCenter, tScale, tColor, UiTheme.Shadow);

            // items
            for (int i = 0; i < _items.Count; i++)
            {
                var it = _items[i];
                bool hovered = IsMouseOver(it);
                it.Selected = (i == _selected);

                float scale = 1f;
                Color color = UiTheme.Text;

                if (it.Selected)
                {
                    float p = 0.1f * (float)System.Math.Sin(_pulse * 4f);
                    scale = 1.1f + p;
                    color = Color.Lerp(Color.Yellow, Color.Orange, 0.5f + p);
                }
                else if (hovered)
                {
                    scale = 1.05f;
                    color = Color.Lerp(UiTheme.Text, Color.LightBlue, UiAnim.Osc(_hover));
                }

                TextRenderer.DrawCentered(sb, _font, it.Text, it.Center, scale, color);
            }

            // simpele cursor
            sb.DrawString(_font, ">", new Vector2(_mouse.X - 15, _mouse.Y - 5), UiTheme.Text);
            sb.DrawString(_font, "<", new Vector2(_mouse.X + 5, _mouse.Y - 5), UiTheme.Text);
        }

        private string DifficultyLabel() => $"Moeilijkheid: {_difficulty}";
        private void ToggleDifficulty() =>
            _difficulty = _difficulty == Difficulty.Moeilijk ? Difficulty.Makkelijk : Difficulty.Moeilijk;

        private bool IsMouseOver(MenuItem item)
        {
            var size = _font.MeasureString(item.Text);
            var rect = new Rectangle(
                (int)(item.Center.X - size.X * 0.5f),
                (int)(item.Center.Y - size.Y * 0.5f),
                (int)size.X, (int)size.Y);
            return rect.Contains(_mouse.Position);
        }
    }
}