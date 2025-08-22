using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Ai
{
    public class WanderAi : IEnemyAI
    {
        private Vector2 _dir = Vector2.Zero;
        private float _timer;
        private readonly float _changeEvery;
        private readonly Random _rng;

        public WanderAi(float changeEverySeconds = 2.0f, int? seed = null)
        {
            _changeEvery = Math.Max(0.2f, changeEverySeconds);
            _rng = seed.HasValue ? new Random(seed.Value) : new Random();
            PickNewDirection();
        }

        public Vector2 ComputeDesiredVelocity(Vector2 enemyPos, Vector2 heroPos, float maxSpeed, float dt)
        {
            _timer += dt;
            if (_timer >= _changeEvery)
            {
                _timer = 0f;
                PickNewDirection();
            }
            return _dir * (maxSpeed * 0.6f); // wander wat trager
        }

        private void PickNewDirection()
        {
            var x = (float)(_rng.NextDouble() * 2 - 1);
            var y = (float)(_rng.NextDouble() * 2 - 1);
            var v = new Vector2(x, y);
            _dir = v == Vector2.Zero ? new Vector2(1, 0) : Vector2.Normalize(v);
        }

        
    }
}
