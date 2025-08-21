using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Imenyaan.Entities
{
    public class Obstacle
    {
        public Vector2 Position;      // tekenpositie (linksboven, tenzij je Origin aanpast)
        public Rectangle Collider;    // bots-rect (wereldcoördinaten)

        private readonly string _asset;
        private readonly Vector2 _drawOffset;
        private readonly bool _autoScaleToCollider;
        private readonly float _uniformScaleIfNotAuto; // fallback als autoscale uit staat

        public SimpleSprite Sprite { get; private set; }

        public Obstacle(
            string asset,
            Vector2 position,
            Rectangle collider,
            bool autoScaleToCollider = true,
            Vector2? drawOffset = null,
            float uniformScaleIfNotAuto = 1f)
        {
            _asset = asset;
            Position = position;
            Collider = collider;
            _autoScaleToCollider = autoScaleToCollider;
            _drawOffset = drawOffset ?? Vector2.Zero;
            _uniformScaleIfNotAuto = uniformScaleIfNotAuto;
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = new SimpleSprite();
            Sprite.Load(content, _asset);
            Sprite.Origin = Vector2.Zero; // of bv. middenonder: new Vector2(Sprite.SizePixels.X/2f, Sprite.SizePixels.Y);

            if (_autoScaleToCollider)
            {
                var tex = Sprite.SizePixels; // originele textuurgrootte in pixels
                // voorkom delen door 0
                float sx = tex.X > 0 ? (float)Collider.Width / tex.X : 1f;
                float sy = tex.Y > 0 ? (float)Collider.Height / tex.Y : 1f;
                Sprite.Scale = new Vector2(sx, sy);
            }
            else
            {
                Sprite.Scale = new Vector2(_uniformScaleIfNotAuto, _uniformScaleIfNotAuto);
            }
        }

        public void Draw(SpriteBatch sb, bool debug = false, Texture2D debugPixel = null)
        {
            // teken sprite; Position is gekoppeld aan collider-linksboven
            Sprite.Draw(sb, Position - _drawOffset);

            if (debug && debugPixel != null)
                sb.Draw(debugPixel, Collider, Color.LimeGreen * 0.25f);
        }
    }
}
