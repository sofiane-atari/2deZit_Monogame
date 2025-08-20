using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities
{
    public class Hero
    {
        public Vector2 Position { get; private set; }
        private Vector2 _velocity;

        private AnimatedSprite _walkAnim;

        // Voeg een variabele toe voor de schaalfactor
        private const float _scale = 0.15f;

        private const float Acceleration = 600f;
        private const float MaxSpeed = 150f;
        private const float Friction = 500f;

        private bool _facingLeft;

        public void LoadContent(ContentManager content)
        {
            _walkAnim = new AnimatedSprite();
            _walkAnim.Load(content, "Sprites/hero_walk", 322, 373, 3, 0.15f);
            Position = new Vector2(200, 200);
        }

        public void Update(GameTime gameTime, KeyboardState kb)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                // frictie
                if (_velocity.Length() > 0)
                {
                    var frictionForce = Friction * dt;
                    if (_velocity.Length() <= frictionForce)
                        _velocity = Vector2.Zero;
                    else
                        _velocity -= Vector2.Normalize(_velocity) * frictionForce;
                }
            }

            // max speed
            if (_velocity.Length() > MaxSpeed)
                _velocity = Vector2.Normalize(_velocity) * MaxSpeed;

            Position += _velocity * dt;
        }

        public void Draw(SpriteBatch sb)
        {
            // Pas de Draw-aanroep aan om de schaalfactor mee te geven
            _walkAnim.Draw(sb, Position, _scale, _facingLeft);
        }
    }
}
