using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Controller
{
    public class KeyboardInputController : IInputController
    {
        private readonly Keys _up, _down, _left, _right;

        public KeyboardInputController(Keys up, Keys down, Keys left, Keys right)
        {
            _up = up; _down = down; _left = left; _right = right;
        }

        public Vector2 GetMove()
        {
            var kb = Keyboard.GetState();
            Vector2 v = Vector2.Zero;
            if (kb.IsKeyDown(_up)) v.Y -= 1;
            if (kb.IsKeyDown(_down)) v.Y += 1;
            if (kb.IsKeyDown(_left)) v.X -= 1;
            if (kb.IsKeyDown(_right)) v.X += 1;
            if (v != Vector2.Zero) v.Normalize();
            return v;
        }
    }
}
