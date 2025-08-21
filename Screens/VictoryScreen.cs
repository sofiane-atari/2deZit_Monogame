using Imenyaan.Core;
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
    public class VictoryScreen : GameScreen
    {
        private SpriteFont _font;
        private KeyboardState _prevKb;

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            bool Pressed(Keys k) => kb.IsKeyDown(k) && !_prevKb.IsKeyDown(k);

            if (Pressed(Keys.Enter))
                Screens.ChangeScreen(new StartScreen());

            _prevKb = kb;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            var vp = Game.GraphicsDevice.Viewport;
            var title = "VICTORY!";
            var sub = "Enter = Terug naar menu";

            var sT = _font.MeasureString(title);
            var sS = _font.MeasureString(sub);

            var posT = new Vector2((vp.Width - sT.X) * 0.5f, (vp.Height - sT.Y) * 0.4f);
            var posS = new Vector2((vp.Width - sS.X) * 0.5f, posT.Y + sT.Y + 20);

            sb.DrawString(_font, title, posT, Color.Gold);
            sb.DrawString(_font, sub, posS, Color.White);
        }
    }
}
