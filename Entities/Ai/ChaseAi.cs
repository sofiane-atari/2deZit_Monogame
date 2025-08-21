using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Ai
{
    public class ChaseAi : IEnemyAI
    {
        public Vector2 ComputeDesiredVelocity(Vector2 enemyPos, Vector2 heroPos, float maxSpeed, float dt)
        {
            var dir = heroPos - enemyPos;
            if (dir == Vector2.Zero) return Vector2.Zero;
            dir.Normalize();
            return dir * maxSpeed;
        }

        
    }
}
