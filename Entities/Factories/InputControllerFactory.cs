using Imenyaan.Entities.Controller;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Factories
{
    public static class InputControllerFactory
    {
        public static IInputController CreateWASD()
            => new KeyboardInputController(Keys.W, Keys.S, Keys.A, Keys.D);

        public static IInputController CreateArrows()
            => new KeyboardInputController(Keys.Up, Keys.Down, Keys.Left, Keys.Right);

        
    }
}
