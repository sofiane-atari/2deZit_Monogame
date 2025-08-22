using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public sealed class WrapBehavior : IParticleBehavior
    {
        public void Update(ref Particle p, float dt, Rectangle b)
        {
            p.Position += p.Velocity * 50f * dt;
            p.Rotation += p.RotationSpeed;

            if (p.Position.X < 0) p.Position.X = b.Width;
            if (p.Position.X > b.Width) p.Position.X = 0;
            if (p.Position.Y < 0) p.Position.Y = b.Height;
            if (p.Position.Y > b.Height) p.Position.Y = 0;
        }
    }
}
