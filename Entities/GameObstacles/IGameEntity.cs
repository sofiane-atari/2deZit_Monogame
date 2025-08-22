using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.GameObstacles
{
    public interface IGameEntity
    {
        Vector2 Position { get; }
        Rectangle Collider { get; }
        void LoadContent(ContentManager content);
        void Draw(SpriteBatch spriteBatch);
    }
}
