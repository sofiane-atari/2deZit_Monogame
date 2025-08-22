using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public sealed class MenuItem
    {
        public string Text { get; set; }
        public Action OnActivate { get; }
        public Vector2 Center { get; set; }
        public bool Selected { get; set; }

        public MenuItem(string text, Action onActivate, Vector2 center)
        { Text = text; OnActivate = onActivate; Center = center; }
    }
}
