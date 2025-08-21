using Imenyaan.Entities.Controller;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities
{
    public sealed record HeroConfig
    {
        // Verplicht
        public IInputController Controller { get; init; }
        public string SpriteAsset { get; init; }
        public int FrameW { get; init; }
        public int FrameH { get; init; }
        public int FrameCount { get; init; }
        public float FrameTime { get; init; }
        public int FramesPerRow { get; init; } = 0; // 0 = strip
        public int StartFrame { get; init; } = 0;

        // Movement (defaults)
        public float Accel { get; init; } = 600f;
        public float MaxSpeed { get; init; } = 150f;
        public float Friction { get; init; } = 500f;

        // Visuals 
        public int TargetHeightPx { get; init; } = 0;
        public float Scale { get; init; } = 1f;
        public Vector2 DrawOffset { get; init; } = new(8, 18);

        // Collider (0 => auto)
        public int HitboxW { get; init; } = 0;
        public int HitboxH { get; init; } = 0;

        // Misc
        public float InvulDuration { get; init; } = 0.8f;
        public Vector2 StartPos { get; init; } = new(200, 200);

        // Handige helpers
        public static HeroConfig ForKeyboard(
            IInputController controller,
            string spriteAsset, int fw, int fh, int count, float ft, int fpr = 0) =>
            new()
            {
                Controller = controller,
                SpriteAsset = spriteAsset,
                FrameW = fw,
                FrameH = fh,
                FrameCount = count,
                FrameTime = ft,
                FramesPerRow = fpr
            };
    }
}
