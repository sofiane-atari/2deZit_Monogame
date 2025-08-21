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

        //property voor invul zichtbaar
        public bool IsInvulnerable => _invulTime > 0f;

    
        private const int HitboxWidth = 40;
        private const int HitboxHeight = 50;

        private AnimatedSprite _walkAnim;
        private bool _facingLeft;
        private float _scale;

        private float _invulTime = 0f;
        private const float InvulDuration = 1f;

        // ---- Nieuw: config + input + sheet-parameters ----
        private readonly HeroConfig _cfg;
        private readonly IInputController _controller;

        private readonly string _asset;
        private readonly int _frameW, _frameH, _frameCount, _framesPerRow, _startFrame;
        private readonly float _frameTime;

        public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, HitboxWidth, HitboxHeight);

        // Constructor: haalt alleen input + sprite info uit cfg
        public Hero(HeroConfig cfg)
        {
            _cfg = cfg;
            _controller = cfg.Controller;
            Position = _cfg.StartPos;

            _asset = _cfg.SpriteAsset;
            _frameW = _cfg.FrameW;
            _frameH = _cfg.FrameH;
            _frameCount = _cfg.FrameCount;
            _frameTime = _cfg.FrameTime;
            _framesPerRow = _cfg.FramesPerRow;
            _startFrame = _cfg.StartFrame;

            // BEREKEN SCHAAL op basis van TargetHeightPx
            if (_cfg.TargetHeightPx > 0)
            {
                _scale = _cfg.TargetHeightPx / (float)_frameH;
            }
            else
            {
                _scale = _cfg.Scale;
            }
        }

        public void LoadContent(ContentManager content)
        {
            _walkAnim = new AnimatedSprite();
            _walkAnim.Load(content, _asset, _frameW, _frameH, _frameCount, _frameTime, _framesPerRow, _startFrame);
        }

        // input komt uit _controller (encapsulated)
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
                // versnellen + anim updaten
                _velocity += input * Accel * dt;

                // facing
                if (input.X < -0.01f) _facingLeft = true;
                else if (input.X > 0.01f) _facingLeft = false;

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
                return true;                    // net gestorven
            }

            _invulTime = InvulDuration;
            return false;                       // geraakt maar leeft nog
        }

        public void Draw(SpriteBatch sb)
        {
            // Flicker tijdens i-frames
            bool flicker = _invulTime > 0f && ((int)(_invulTime * 20f) % 2 == 0);
            if (flicker) return;

            // Visuele offset zodat sprite en hitbox mooi uitlijnen 
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
