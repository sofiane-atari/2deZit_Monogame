using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public static class UiTheme
    {
        public static readonly Color BgDark = new(20, 30, 50);
        public static readonly Color Title1 = Color.Gold;
        public static readonly Color Title2 = Color.Orange;
        public static readonly Color Text = Color.White;
        public static readonly Color Shadow = Color.Black * 0.5f;

        public const float TitleBaseScale = 1.5f;
        public const float TitlePulseAmp = 0.1f;
    }
}
