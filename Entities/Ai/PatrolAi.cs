using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Ai
{
    public class PatrolAi : IEnemyAI
    {
        private readonly Vector2 _a;
        private readonly Vector2 _b;
        private bool _toB = true;
        private const float ReachEps = 4f;

        public PatrolAi(Vector2 pointA, Vector2 pointB)
        {
            _a = pointA; _b = pointB;
        }

        public Vector2 ComputeDesiredVelocity(Vector2 enemyPos, Vector2 heroPos, float maxSpeed, float dt)
        {
            var target = _toB ? _b : _a;
            var to = target - enemyPos;
            if (to.LengthSquared() <= ReachEps * ReachEps)
                _toB = !_toB;

            if (to == Vector2.Zero) return Vector2.Zero;
            to.Normalize();
            return to * (maxSpeed * 0.8f);
        }
    }
}
