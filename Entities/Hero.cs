using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Imenyaan.Entities
{
    public class Hero
    {
        public Vector2 Position { get; private set; }
        private Vector2 _velocity;

        private AnimatedSprite _walkAnim;

        private const float _scale = 0.15f;

        private const float Acceleration = 600f;
        private const float MaxSpeed = 150f;
        private const float Friction = 500f;

        private bool _facingLeft;

        private const int HitboxWidth = 40;   // experimenteer: 35–50
        private const int HitboxHeight = 50;

        private Rectangle HitboxAt(Vector2 pos) =>
            new Rectangle((int)pos.X, (int)pos.Y, HitboxWidth, HitboxHeight);
        public void LoadContent(ContentManager content)
        {
            _walkAnim = new AnimatedSprite();
            _walkAnim.Load(content, "Sprites/hero_walk", 322, 373, 3, 0.15f);
            Position = new Vector2(200, 200);
        }

        public void UpdateWithCollision(GameTime gameTime,KeyboardState kb,
            IEnumerable<Rectangle> colliders)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // input
            Vector2 input = Vector2.Zero;
            if (kb.IsKeyDown(Keys.Up)) input.Y -= 1;
            if (kb.IsKeyDown(Keys.Down)) input.Y += 1;
            if (kb.IsKeyDown(Keys.Left)) { input.X -= 1; _facingLeft = true; }
            if (kb.IsKeyDown(Keys.Right)) { input.X += 1; _facingLeft = false; }

            if (input != Vector2.Zero)
            {
                input.Normalize();
                _velocity += input * Acceleration * dt;
                _walkAnim.Update(gameTime);
            }
            else
            {
                if (_velocity.Length() > 0)
                {
                    var f = Friction * dt;
                    if (_velocity.Length() <= f) _velocity = Vector2.Zero;
                    else _velocity -= Vector2.Normalize(_velocity) * f;
                }
            }

            if (_velocity.Length() > MaxSpeed)
                _velocity = Vector2.Normalize(_velocity) * MaxSpeed;

            // Axis-separated
            Vector2 tryX = new Vector2(Position.X + _velocity.X * dt, Position.Y);
            if (!Collides(HitboxAt(tryX), colliders)) Position = tryX;
            else _velocity.X = 0;

            Vector2 tryY = new Vector2(Position.X, Position.Y + _velocity.Y * dt);
            if (!Collides(HitboxAt(tryY), colliders)) Position = tryY;
            else _velocity.Y = 0;
        }

        private static bool Collides(Rectangle r, IEnumerable<Rectangle> colliders)
            => colliders.Any(c => r.Intersects(c));

        private static bool IntersectsAny(Rectangle rect, List<Rectangle> boxes)
        {
            for (int i = 0; i < boxes.Count; i++)
                if (rect.Intersects(boxes[i])) return true;
            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            _walkAnim.Draw(sb, Position, _scale, _facingLeft);

            // Debug: hitbox tonen (tijdelijk)
            // var pixel = new Texture2D(sb.GraphicsDevice, 1, 1);
            // pixel.SetData(new[] { Color.White });
            // sb.Draw(pixel, HitboxAt(Position), Color.Red * 0.25f);
        }
    }
}
