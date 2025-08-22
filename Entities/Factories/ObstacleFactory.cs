using Imenyaan.Entities.Definitions;
using Imenyaan.Entities.GameEntities;
using Imenyaan.Entities.GameObstacles;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Factories
{
        public static class ObstacleFactory
        {
            public static List<Obstacle> BuildAll(IEnumerable<ObstacleDefinition> defs, ContentManager content)
            {
                var list = new List<Obstacle>();

                foreach (var d in defs)
                {
                    
                    IScalingStrategy strategy = d.AutoScaleToCollider
                        ? new AutoScalingStrategy()
                        : new BottomAlignUniformStrategy(d.UniformScaleIfNotAuto);

                    
                    var o = new Obstacle(
                        asset: d.Asset,
                        position: d.Position,
                        collider: d.Collider,
                        scaling: strategy,
                        drawOffset: d.DrawOffset
                    );

                    o.LoadContent(content);
                    list.Add(o);
                }

                return list;
            }
        }
    }
