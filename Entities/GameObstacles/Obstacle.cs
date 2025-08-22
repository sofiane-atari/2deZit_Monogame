using Imenyaan.Entities.GameEntities;
using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Imenyaan.Entities.GameObstacles
{
    public class Obstacle : IGameEntity
    {
        public Vector2 Position { get; private set; }
        public Rectangle Collider { get; private set; }
        public SimpleSprite Sprite { get; private set; }

        private readonly string _asset;
        private readonly IScalingStrategy _scaling;
        private readonly Vector2 _drawOffset;

        public Obstacle(string asset, Vector2 position, Rectangle collider, IScalingStrategy scaling, Vector2 drawOffset)
        {
            _asset = asset;
            Position = position;
            Collider = collider;
            _scaling = scaling;
            _drawOffset = drawOffset;
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = new SimpleSprite();
            Sprite.Load(content, _asset);
            Sprite.Origin = Vector2.Zero;

            var (scale, pos) = _scaling.Calculate(Sprite, Collider, _drawOffset);
            Sprite.Scale = scale;
            Position = pos;
        }

        public void Draw(SpriteBatch sb, bool debug = false, Texture2D debugPixel = null)
        {
            Sprite.Draw(sb, Position);
            if (debug && debugPixel != null)
                sb.Draw(debugPixel, Collider, Color.LimeGreen * 0.25f);
        }

        // IGameEntity
        void IGameEntity.LoadContent(ContentManager content) => LoadContent(content);
        void IGameEntity.Draw(SpriteBatch spriteBatch) => Draw(spriteBatch);
    }
}
