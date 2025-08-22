using Imenyaan.Entities.Factories;
using Imenyaan.Entities.GameObstacles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities
{
    public sealed class World
    {
        public Texture2D Background { get; private set; }
        public Rectangle Bounds { get; private set; }
        public List<Obstacle> Obstacles { get; private set; }
        private Texture2D _debugPixel;

        public void Load(ContentManager content, GraphicsDevice gd, string bgAsset,
                         IEnumerable<Imenyaan.Entities.Definitions.ObstacleDefinition> defs)
        {
            Background = content.Load<Texture2D>(bgAsset);
            var vp = gd.Viewport;
            Bounds = new Rectangle(0, 0, vp.Width, vp.Height);

            Obstacles = ObstacleFactory.BuildAll(defs, content);

            _debugPixel = new Texture2D(gd, 1, 1);
            _debugPixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Background, new Rectangle(0, 0, Bounds.Width, Bounds.Height), Color.White);
            foreach (var o in Obstacles)
                o.Draw(sb, debug: false, debugPixel: _debugPixel);
        }
    }
}
