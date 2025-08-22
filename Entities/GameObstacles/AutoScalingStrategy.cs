using Imenyaan.Entities.GameEntities;
using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.GameObstacles
{
    public sealed class AutoScalingStrategy : IScalingStrategy
    {
        public (Vector2 Scale, Vector2 Position) Calculate(SimpleSprite sprite, Rectangle collider, Vector2 drawOffset)
        {
            var tex = sprite.SizePixels;
            float sx = tex.X > 0 ? (float)collider.Width / tex.X : 1f;
            float sy = tex.Y > 0 ? (float)collider.Height / tex.Y : 1f;
            return (new Vector2(sx, sy), new Vector2(collider.X, collider.Y) + drawOffset);
        }
    }
}
