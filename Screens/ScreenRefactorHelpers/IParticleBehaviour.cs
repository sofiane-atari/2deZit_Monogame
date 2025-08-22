using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public interface IParticleBehavior
    {
        void Update(ref Particle p, float dt, Rectangle bounds);
    }
}
