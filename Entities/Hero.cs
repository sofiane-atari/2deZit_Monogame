using Imenyaan.Entities.Controller;
using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

        public bool IsInvulnerable => _invulTime > 0f;

        // vaste hitbox 
        private const int HitboxWidth = 40;
        private const int HitboxHeight = 50;

        private AnimatedSprite _walkAnim;
        private bool _facingLeft;
        private float _scale;

        private float _invulTime = 0f;
        private const float InvulDuration = 1f;

        // Config + input + anim
        private readonly HeroConfig _cfg;
        private readonly IInputController _controller;
        private readonly AnimationDesc _animDesc;

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, HitboxWidth, HitboxHeight);

        public Hero(HeroConfig cfg)
        {
            _cfg = cfg;
            _controller = cfg.Controller;
            Position = _cfg.StartPos;

            
            _animDesc = new AnimationDesc(
                _cfg.SpriteAsset,
                _cfg.FrameW, _cfg.FrameH,
                _cfg.FrameCount, _cfg.FrameTime,
                _cfg.FramesPerRow, _cfg.StartFrame
            );

            
            _scale = _cfg.TargetHeightPx > 0
                ? _cfg.TargetHeightPx / (float)_animDesc.FrameHeight
                : _cfg.Scale;
        }

        public void LoadContent(ContentManager content)
        {
            _walkAnim = new AnimatedSprite();
            _walkAnim.Load(content, _animDesc); 
        }

        public void UpdateWithCollision(GameTime gameTime,
                                        IEnumerable<Rectangle> obstacles,
                                        Rectangle worldBounds)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_invulTime > 0f) _invulTime -= dt;

            // Input via controller
            Vector2 input = _controller.GetMove();

            if (input != Vector2.Zero)
            {
                _velocity += input * Accel * dt;

                if (input.X < -0.01f) _facingLeft = true;
                else if (input.X > 0.01f) _facingLeft = false;

                _walkAnim.Update(gameTime);
            }
            else
            {
                if (_velocity.Length() > 0f)
                {
                    float f = Friction * dt;
                    if (_velocity.Length() <= f) _velocity = Vector2.Zero;
                    else _velocity -= Vector2.Normalize(_velocity) * f;
                }
            }

            if (_velocity.Length() > MaxSpeed)
                _velocity = Vector2.Normalize(_velocity) * MaxSpeed;

            // X
            float newX = MathHelper.Clamp(Position.X + _velocity.X * dt, worldBounds.Left, worldBounds.Right - HitboxWidth);
            var tryX = new Vector2(newX, Position.Y);
            if (!IntersectsAny(HitboxAt(tryX), obstacles)) Position = tryX;
            else _velocity.X = 0;

            // Y
            float newY = MathHelper.Clamp(Position.Y + _velocity.Y * dt, worldBounds.Top, worldBounds.Bottom - HitboxHeight);
            var tryY = new Vector2(Position.X, newY);
            if (!IntersectsAny(HitboxAt(tryY), obstacles)) Position = tryY;
            else _velocity.Y = 0;
        }

        public bool TakeHit()
        {
            if (IsDead) return true;
            if (_invulTime > 0f) return false;

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
            bool flicker = _invulTime > 0f && ((int)(_invulTime * 20f) % 2 == 0);
            if (flicker) return;

            // Gebruik je config-offset i.p.v. hardcoded
            var drawOffset = _cfg.DrawOffset;
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
