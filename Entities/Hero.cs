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
        public Vector2 Position { get; private set; } = new Vector2(200, 200);

        private Vector2 _velocity;
        private const float Accel = 600f;
        private const float MaxSpeed = 150f;
        private const float Friction = 500f;

        private int _lives = 3;
        public int Lives => _lives;              
        public bool IsDead { get; private set; } 

        // (optioneel) property voor invul zichtbaar
        public bool IsInvulnerable => _invulTime > 0f;

        // Hitbox (afstemmen op jouw sprite + scale)
        private const int HitboxWidth = 40;
        private const int HitboxHeight = 50;

        // Animatie (jouw AnimatedSprite wrapper met Draw(position, scale, flipX))
        private AnimatedSprite _walkAnim;
        private bool _facingLeft;
        private const float _scale = 0.15f;

        // i-frames (optioneel voor extra)
        private float _invulTime = 0f;
        private const float InvulDuration = 0.8f;

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, HitboxWidth, HitboxHeight);

        public void LoadContent(ContentManager content)
        {
            _walkAnim = new AnimatedSprite();
            _walkAnim.Load(content, "Sprites/hero_walk", 322, 373, 3, 0.12f);
        }

        public void UpdateWithCollision(GameTime gameTime, KeyboardState kb,
                                        IEnumerable<Rectangle> obstacles, Rectangle worldBounds)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_invulTime > 0f) _invulTime -= dt;

            // Input
            Vector2 input = Vector2.Zero;
            if (kb.IsKeyDown(Keys.Up)) input.Y -= 1;
            if (kb.IsKeyDown(Keys.Down)) input.Y += 1;
            if (kb.IsKeyDown(Keys.Left)) { input.X -= 1; _facingLeft = true; }
            if (kb.IsKeyDown(Keys.Right)) { input.X += 1; _facingLeft = false; }

            if (input != Vector2.Zero)
            {
                input.Normalize();
                _velocity += input * Accel * dt;
                _walkAnim.Update(gameTime);
            }
            else
            {
                // Frictie
                if (_velocity.Length() > 0f)
                {
                    float f = Friction * dt;
                    if (_velocity.Length() <= f) _velocity = Vector2.Zero;
                    else _velocity -= Vector2.Normalize(_velocity) * f;
                }
            }

            // Max snelheid
            if (_velocity.Length() > MaxSpeed)
                _velocity = Vector2.Normalize(_velocity) * MaxSpeed;

            // ---- Axis-separated + WORLD CLAMP ----
            // X
            float newX = Position.X + _velocity.X * dt;
            newX = MathHelper.Clamp(newX, worldBounds.Left, worldBounds.Right - HitboxWidth);
            var tryX = new Vector2(newX, Position.Y);
            if (!IntersectsAny(HitboxAt(tryX), obstacles)) Position = tryX;
            else _velocity.X = 0;

            // Y
            float newY = Position.Y + _velocity.Y * dt;
            newY = MathHelper.Clamp(newY, worldBounds.Top, worldBounds.Bottom - HitboxHeight);
            var tryY = new Vector2(Position.X, newY);
            if (!IntersectsAny(HitboxAt(tryY), obstacles)) Position = tryY;
            else _velocity.Y = 0;
        }

        public bool TakeHit()
        {
            if (IsDead) return true;            // al dood
            if (_invulTime > 0f) return false;  // nog onschendbaar

            _lives--;
            if (_lives <= 0)
            {
                _lives = 0;
                IsDead = true;
                return true;                    
            }

            _invulTime = InvulDuration;
            return false;                       
        }

        public void Draw(SpriteBatch sb)
        {
            // Flicker tijdens i-frames
            bool flicker = _invulTime > 0f && ((int)(_invulTime * 20f) % 2 == 0);
            if (flicker) return;

            // Eventuele visuele offset zodat sprite en hitbox mooi uitlijnen
            var drawOffset = new Vector2(8, 18);
            _walkAnim.Draw(sb, Position - drawOffset, _scale, _facingLeft);
        }

        private Rectangle HitboxAt(Vector2 pos) => new Rectangle((int)pos.X, (int)pos.Y, HitboxWidth, HitboxHeight);

        private static bool IntersectsAny(Rectangle r, IEnumerable<Rectangle> boxes)
        {
            foreach (var b in boxes) if (r.Intersects(b)) return true;
            return false;
        }
    }
}
