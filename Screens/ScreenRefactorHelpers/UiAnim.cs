using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public static class UiAnim
    {
        public static float Pulse(float t, float amp = 0.1f) => 1f + amp * (float)Math.Sin(t);
        public static float Osc(float t) => 0.5f + 0.5f * (float)Math.Sin(t);
    }
}
