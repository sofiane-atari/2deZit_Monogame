using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Imenyaan.Entities
{
    public class Obstacle
    {
        public Vector2 Position;    // BEREKENDE tekenpositie
        public Rectangle Collider;  // Bots-rect

        private readonly string _asset;
        private readonly bool _autoScaleToCollider;
        private readonly Vector2 _drawOffset;          // je had dit al
        private readonly float _uniformScaleIfNotAuto; // fallback

        public SimpleSprite Sprite { get; private set; }

        public Obstacle(
            string asset,
            Vector2 position,
            Rectangle collider,
            bool autoScaleToCollider,
            Vector2 drawOffset,
            float uniformScaleIfNotAuto = 1f)
        {
            _asset = asset;
            Position = position;              // wordt zo meteen overschreven
            Collider = collider;
            _autoScaleToCollider = autoScaleToCollider;
            _drawOffset = drawOffset;
            _uniformScaleIfNotAuto = uniformScaleIfNotAuto;
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = new SimpleSprite();
            Sprite.Load(content, _asset);
            Sprite.Origin = Vector2.Zero;

            if (_autoScaleToCollider)
            {
                // 1) Sprite exact schalen naar collider
                var texSize = Sprite.SizePixels; // oorspronkelijke texture
                float sx = texSize.X > 0 ? (float)Collider.Width / texSize.X : 1f;
                float sy = texSize.Y > 0 ? (float)Collider.Height / texSize.Y : 1f;
                Sprite.Scale = new Vector2(sx, sy);

                // 2) Positie gelijk aan collider.Left/Top (eventueel + drawOffset)
                Position = new Vector2(Collider.X, Collider.Y) + _drawOffset;
            }
            else
            {
                // 1) Hou eigen (uniforme) schaal
                Sprite.Scale = new Vector2(_uniformScaleIfNotAuto, _uniformScaleIfNotAuto);

                // 2) Align onderkant sprite op collider.Bottom (Bottom-Left), dan optioneel offset
                var size = Sprite.SizeOnScreen; // na schaal
                Position = new Vector2(Collider.Left, Collider.Bottom - size.Y) + _drawOffset;
            }
        }

        public void Draw(SpriteBatch sb, bool debug = false, Texture2D debugPixel = null)
        {
            Sprite.Draw(sb, Position);
            if (debug && debugPixel != null)
                sb.Draw(debugPixel, Collider, Color.LimeGreen * 0.25f);
        }
    }
}
