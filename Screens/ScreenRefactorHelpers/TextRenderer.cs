using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public static class TextRenderer
    {
        
        public static void DrawCentered(SpriteBatch sb, SpriteFont font,
                                        string text, Vector2 center,
                                        float scale, Color color, Color? shadow = null)
        {
            var size = font.MeasureString(text);
            var origin = size * 0.5f;
            if (shadow is Color sh)
                sb.DrawString(font, text, center + new Vector2(2, 2), sh, 0f, origin, scale, 0, 0);
            sb.DrawString(font, text, center, color, 0f, origin, scale, 0, 0);
        }
    }
}
