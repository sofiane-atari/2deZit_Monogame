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
    public sealed class BottomAlignUniformStrategy : IScalingStrategy
    {
        private readonly float _uniformScale;
        public BottomAlignUniformStrategy(float uniformScale) => _uniformScale = uniformScale;

        public (Vector2 Scale, Vector2 Position) Calculate(SimpleSprite sprite, Rectangle collider, Vector2 drawOffset)
        {
            var scale = new Vector2(_uniformScale, _uniformScale);
            var sizeOnScreen = new Point(
                (int)(sprite.SizePixels.X * scale.X),
                (int)(sprite.SizePixels.Y * scale.Y)
            );
            var pos = new Vector2(collider.Left, collider.Bottom - sizeOnScreen.Y) + drawOffset;
            return (scale, pos);
        }
    }
}
