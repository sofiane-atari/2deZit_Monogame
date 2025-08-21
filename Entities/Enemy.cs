using Imenyaan.Entities.Ai;
using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Imenyaan.Entities
{
    public class Enemy
    {
        public Vector2 Position { get; private set; }
        public Rectangle Collider => new Rectangle((int)Position.X, (int)Position.Y, _hitboxW, _hitboxH);

        private readonly IEnemyAI _ai;
        private readonly AnimationDesc _animDesc;

        private int _hitboxW, _hitboxH;
        private float _scale;               // niet readonly meer

        private readonly int _targetHeightPx; // als >0: schaal = targetHeight / frameHeight
        private readonly int _targetWidthPx;  // als >0: schaal = targetWidth  / frameWidth

        private readonly float _maxSpeed;
        private readonly Vector2 _drawOffset;

        private Vector2 _velocity;
        private readonly float _accel = 500f, _friction = 500f;
        private bool _faceLeft;
        private AnimatedSprite _anim;

        public Enemy(
            IEnemyAI ai,
            AnimationDesc animDesc,
            Vector2 startPos,
            int hitboxW, int hitboxH,
            float maxSpeed = 110f,
            float scale = 1f,                 // fallback als je géén targetHeight/Width geeft
            Vector2? drawOffset = null,
            int targetHeightPx = 0,           // <<< NIEUW
            int targetWidthPx = 0            // <<< NIEUW
        )
        {
            _ai = ai;
            _animDesc = animDesc;
            Position = startPos;

            _hitboxW = hitboxW;
            _hitboxH = hitboxH;

            _maxSpeed = maxSpeed;
            _scale = scale;
            _drawOffset = drawOffset ?? Vector2.Zero;

            _targetHeightPx = targetHeightPx;
            _targetWidthPx = targetWidthPx;
        }

        public void LoadContent(ContentManager content)
        {
            _anim = new AnimatedSprite();
            _anim.Load(content, _animDesc.Asset,
                       _animDesc.FrameWidth, _animDesc.FrameHeight,
                       _animDesc.FrameCount, _animDesc.FrameTime,
                       _animDesc.StartFrame);

            // --- Auto SCALE op basis van gewenste schermgrootte ---
            if (_targetHeightPx > 0)
                _scale = _targetHeightPx / (float)_animDesc.FrameHeight;
            else if (_targetWidthPx > 0)
                _scale = _targetWidthPx / (float)_animDesc.FrameWidth;
            // anders laat hij de meegegeven 'scale' staan

            // --- Auto HITBOX als 0 meegegeven ---
            if (_hitboxW == 0 || _hitboxH == 0)
            {
                // vuistregels; tune desnoods iets naar jouw sprites
                _hitboxW = (int)(_animDesc.FrameWidth * _scale * 0.6f);
                _hitboxH = (int)(_animDesc.FrameHeight * _scale * 0.8f);
            }
        }

        public void Update(GameTime gameTime, Vector2 heroPos,
                           IEnumerable<Rectangle> worldColliders, Rectangle worldBounds)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var desired = _ai.ComputeDesiredVelocity(Position, heroPos, _maxSpeed, dt);

            var delta = desired - _velocity;
            if (delta != Vector2.Zero)
            {
                var step = Vector2.Normalize(delta) * _accel * dt;
                if (step.Length() > delta.Length()) step = delta;
                _velocity += step;
            }
            else
            {
                if (_velocity.Length() > 0f)
                {
                    float f = _friction * dt;
                    if (_velocity.Length() <= f) _velocity = Vector2.Zero;
                    else _velocity -= Vector2.Normalize(_velocity) * f;
                }
            }

            if (_velocity.Length() > _maxSpeed)
                _velocity = Vector2.Normalize(_velocity) * _maxSpeed;

            if (_velocity.X < -1) _faceLeft = true; else if (_velocity.X > 1) _faceLeft = false;
            if (_velocity != Vector2.Zero) _anim.Update(gameTime);

            float newX = MathHelper.Clamp(Position.X + _velocity.X * dt, worldBounds.Left, worldBounds.Right - _hitboxW);
            var tryX = new Vector2(newX, Position.Y);
            if (!IntersectsAny(new Rectangle((int)tryX.X, (int)tryX.Y, _hitboxW, _hitboxH), worldColliders))
                Position = tryX;
            else _velocity.X = 0;

            float newY = MathHelper.Clamp(Position.Y + _velocity.Y * dt, worldBounds.Top, worldBounds.Bottom - _hitboxH);
            var tryY = new Vector2(Position.X, newY);
            if (!IntersectsAny(new Rectangle((int)tryY.X, (int)tryY.Y, _hitboxW, _hitboxH), worldColliders))
                Position = tryY;
            else _velocity.Y = 0;
        }

        public void Draw(SpriteBatch sb)
        {
            _anim.Draw(sb, Position - _drawOffset, _scale, _faceLeft);
        }

        private static bool IntersectsAny(Rectangle r, IEnumerable<Rectangle> list)
        {
            foreach (var b in list) if (r.Intersects(b)) return true;
            return false;
        }
    }
}
