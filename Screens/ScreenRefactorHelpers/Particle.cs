using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public struct Particle
    {
        public Vector2 Position, Velocity;
        public float Rotation, RotationSpeed;
        public int Size;
        public Color Color;
    }
}
