using Imenyaan.Entities;
using Imenyaan.Entities.GameObstacles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Managers
{
    public class CollisionManager
    {
        public bool CheckCoinCollection(Coin coin, params Hero[] heroes)
        {
            return heroes.Any(hero => coin.TryCollect(hero.Hitbox));
        }

        public bool CheckEnemyCollision(Enemy enemy, params Hero[] heroes)
        {
            return heroes.Any(hero => enemy.Collider.Intersects(hero.Hitbox) && hero.TakeHit());
        }
    }
}
