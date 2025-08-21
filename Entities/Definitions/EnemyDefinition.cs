using Imenyaan.Entities.Ai;
using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Definitions
{
    public class EnemyDefinition
    {
        public IEnemyAI AI { get; init; }
        public AnimationDesc Anim { get; init; }
        public Vector2 Position { get; init; }
        public int HitboxW { get; init; }
        public int HitboxH { get; init; }
        public float MaxSpeed { get; init; }
        public float Scale { get; init; }
        public Vector2 DrawOffset { get; init; }
        public int TargetHeightPx { get; init; }
    }
}
