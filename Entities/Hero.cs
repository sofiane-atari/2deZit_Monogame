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
        private Texture2D _texture;

        private const float Acceleration = 600f; // hoe snel hij versnelt
        private const float MaxSpeed = 200f;
        private const float Friction = 500f;     // hoe snel hij afremt

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Hero");
            Position = new Vector2(200, 200);
        }

        public void Update(GameTime gameTime, KeyboardState kb)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 input = Vector2.Zero;

            if (kb.IsKeyDown(Keys.Up)) input.Y -= 1;
            if (kb.IsKeyDown(Keys.Down)) input.Y += 1;
            if (kb.IsKeyDown(Keys.Left)) input.X -= 1;
            if (kb.IsKeyDown(Keys.Right)) input.X += 1;

            // normaliseren zodat diagonaal even snel is
            if (input != Vector2.Zero) input.Normalize();

            // versnellen
            _velocity += input * Acceleration * dt;

            // afremmen (frictie)
            if (input == Vector2.Zero)
            {
                if (_velocity.Length() > 0)
                {
                    var frictionForce = Friction * dt;
                    if (_velocity.Length() <= frictionForce)
                        _velocity = Vector2.Zero;
                    else
                        _velocity -= Vector2.Normalize(_velocity) * frictionForce;
                }
            }

            // max snelheid begrenzen
            if (_velocity.Length() > MaxSpeed)
                _velocity = Vector2.Normalize(_velocity) * MaxSpeed;

            Position += _velocity * dt;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture, Position, Color.White);
        }
    }
}
