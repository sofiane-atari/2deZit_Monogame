using Imenyaan.Entities.Definitions;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Factories
{
    public static class EnemyFactory
    {
        public static Enemy Create(ContentManager content, EnemyDefinition def)
        {
            var e = new Enemy(def.AI, def.Anim, def.Position, def.HitboxW,
                              def.HitboxH, def.MaxSpeed, def.Scale, def.DrawOffset,
                              def.TargetHeightPx);
            e.LoadContent(content);
            return e;
        }
    }
}
