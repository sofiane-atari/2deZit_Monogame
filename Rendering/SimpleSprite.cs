using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Imenyaan.Rendering
{
    public class SimpleSprite
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;

        public void Load(ContentManager content, string asset)
        {
            Texture = content.Load<Texture2D>(asset);
        }

        public void Draw(SpriteBatch sb, Vector2 position, Color? tint = null)
        {
            sb.Draw(Texture, position, null, tint ?? Color.White, 0f, Origin, Scale, SpriteEffects.None, 0f);
        }


        public Point SizePixels => new(Texture?.Width ?? 0, Texture?.Height ?? 0);
        public Point SizeOnScreen => new(
            (int)(SizePixels.X * Scale.X),
            (int)(SizePixels.Y * Scale.Y)
        );
    }

}
