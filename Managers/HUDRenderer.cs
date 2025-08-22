using Imenyaan.Entities;
using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Managers
{
    public class HUDRenderer
    {
        private readonly SpriteFont _font;
        private readonly SimpleSprite _heart;
        private readonly Hero _hero1;
        private readonly Hero _hero2;
        private readonly int _victoryGoal;

        public HUDRenderer(SpriteFont font, SimpleSprite heart, Hero hero1, Hero hero2, int victoryGoal)
        {
            _font = font; _heart = heart; _hero1 = hero1; _hero2 = hero2; _victoryGoal = victoryGoal;
        }

        public void Draw(SpriteBatch spriteBatch, int score)
        {
            spriteBatch.DrawString(_font, "ESC = menu", new Vector2(20, 40), Color.White);
            spriteBatch.DrawString(_font, $"Score: {score}/{_victoryGoal}", new Vector2(20, 100), Color.White);

            DrawHearts(spriteBatch, _hero1.Lives, new Vector2(20, 10));
            DrawHearts(spriteBatch, _hero2.Lives, new Vector2(20, 10 + _heart.SizeOnScreen.Y + 6));
        }

        private void DrawHearts(SpriteBatch sb, int lives, Vector2 pos)
        {
            int spacing = (int)(_heart.SizeOnScreen.X + 6);
            for (int i = 0; i < lives; i++) _heart.Draw(sb, pos + new Vector2(i * spacing, 0));
        }
    }
}
