using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Definitions
{
    public class Level1Definition : ILevelDefinition
    {
        public string BackgroundAsset => "Sprites/Background";

        public IEnumerable<ObstacleDefinition> Obstacles()
        {
            // Links-midden
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(220, 360, 48, 32),
                Position = new Vector2(220, 360),   // = collider.Left/Top
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Midden-boven
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(540, 260, 48, 32),
                Position = new Vector2(540, 260),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Rechts-midden
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(980, 360, 48, 32),
                Position = new Vector2(980, 360),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Links-onder
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(260, 540, 48, 32),
                Position = new Vector2(260, 540),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Rechts-onder
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(980, 540, 48, 32),
                Position = new Vector2(980, 540),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };
        }
    }
}
