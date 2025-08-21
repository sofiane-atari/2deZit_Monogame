using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Rendering
{
    public struct AnimationDesc
    {
        public string Asset;
        public int FrameWidth, FrameHeight;
        public int FrameCount;
        public float FrameTime;
        

        public AnimationDesc(string asset, int fw, int fh, int count, float time)
        {
            Asset = asset; FrameWidth = fw; FrameHeight = fh;
            FrameCount = count; FrameTime = time;

        }
    }
}
