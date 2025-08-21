using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Ai
{
    public interface IEnemyAI
    {
        // Geef de gewenste snelheid (velocity) terug voor deze frame
        Vector2 ComputeDesiredVelocity(Vector2 enemyPos, Vector2 heroPos, float maxSpeed, float dt);
    }
}
