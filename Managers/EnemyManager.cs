using Imenyaan.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Managers
{
    public class EnemyManager
    {
        private readonly List<Enemy> _enemies;

        public EnemyManager(List<Enemy> enemies) => _enemies = enemies;

        public void UpdateEnemies(GameTime gameTime,
            Hero[] heroes, IEnumerable<Rectangle> colliders, Rectangle worldBounds)
        {
            foreach (var enemy in _enemies)
            {
                // Bepaal dichtstbijzijnde hero
                var target = FindNearestHero(enemy, heroes);
                enemy.Update(gameTime, target.Position, colliders, worldBounds);
            }
        }

        public void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (var enemy in _enemies)
                enemy.Draw(spriteBatch);
        }

        private static Hero FindNearestHero(Enemy enemy, Hero[] heroes)
        {
            Hero nearest = heroes[0];
            float minDist = Vector2.DistanceSquared(enemy.Position, nearest.Position);

            for (int i = 1; i < heroes.Length; i++)
            {
                float dist = Vector2.DistanceSquared(enemy.Position, heroes[i].Position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = heroes[i];
                }
            }

            return nearest;
        }
    }
}
