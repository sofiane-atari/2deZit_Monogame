using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Rendering
{
    // Struct goed voor klasses met weinig data
    // Goed voor de performance
    public struct AnimationDesc
    {
        public string Asset;
        public int FrameWidth, FrameHeight;
        public int FrameCount;
        public float FrameTime;
        public int FramesPerRow;   
        public int StartFrame;     

        public AnimationDesc(string asset, int fw, int fh, int count, float time,
                             int framesPerRow = 0, int startFrame = 0)
        {
            Asset = asset;
            FrameWidth = fw;
            FrameHeight = fh;
            FrameCount = count;
            FrameTime = time;
            FramesPerRow = framesPerRow;
            StartFrame = startFrame;
        }
    }
}
