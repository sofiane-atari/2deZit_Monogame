using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens.ScreenRefactorHelpers
{
    public sealed class ParticleSystem
    {
        private readonly List<Particle> _items = new();
        private readonly IParticleBehavior _behavior;
        private readonly Texture2D _pixel;
        private readonly Random _rng;

        public ParticleSystem(Texture2D pixel, IParticleBehavior behavior, Random rng = null)
        { _pixel = pixel; _behavior = behavior; _rng = rng ?? new Random(); }

        public void SpawnConfetti(int count, Rectangle bounds)
        {
            _items.Clear();
            for (int i = 0; i < count; i++)
            {
                _items.Add(new Particle
                {
                    Position = new Vector2(_rng.Next(bounds.Width), _rng.Next(bounds.Height)),
                    Velocity = new Vector2((float)(_rng.NextDouble() * 2 - 1), (float)(_rng.NextDouble() * 2 - 1)),
                    Rotation = (float)(_rng.NextDouble() * MathHelper.TwoPi),
                    RotationSpeed = (float)(_rng.NextDouble() * 0.1f - 0.05f),
                    Size = _rng.Next(3, 8),
                    Color = new Color((float)_rng.NextDouble(), (float)_rng.NextDouble(), (float)_rng.NextDouble())
                });
            }
        }

        public void Update(float dt, Rectangle bounds)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var p = _items[i];
                _behavior.Update(ref p, dt, bounds);
                _items[i] = p;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var p in _items)
            {
                sb.Draw(_pixel, new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Size, p.Size),
                        null, p.Color, p.Rotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
    }
}
