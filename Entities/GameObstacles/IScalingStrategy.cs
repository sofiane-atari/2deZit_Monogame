using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.GameEntities
{
    public interface IScalingStrategy
    {
        (Vector2 Scale, Vector2 Position) Calculate(SimpleSprite sprite, Rectangle collider, Vector2 drawOffset);
    }
}
