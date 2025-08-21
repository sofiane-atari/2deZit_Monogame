using Imenyaan.Entities.Ai;
using Imenyaan.Rendering;
using Imenyaan.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Definitions
{
    public class Level1Definition : ILevelDefinition
    {
        public string BackgroundAsset => "Sprites/Background";

        public IEnumerable<ObstacleDefinition> Obstacles()
        {
            // Links-midden
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(220, 360, 48, 32),
                Position = new Vector2(220, 360),   // = collider.Left/Top
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Midden-boven
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(540, 260, 48, 32),
                Position = new Vector2(540, 260),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Rechts-midden
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(980, 360, 48, 32),
                Position = new Vector2(980, 360),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Links-onder
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(260, 540, 48, 32),
                Position = new Vector2(260, 540),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };

            // Rechts-onder
            yield return new ObstacleDefinition
            {
                Asset = "Props/Crate",
                Collider = new Rectangle(980, 540, 48, 32),
                Position = new Vector2(980, 540),
                AutoScaleToCollider = true,
                DrawOffset = Vector2.Zero,
                UniformScaleIfNotAuto = 1f
            };
        }

        public IEnumerable<EnemyDefinition> Enemies(StartScreen.Difficulty difficulty)
        {
            // Animdescs één keer maken
            var chaserAnim = new AnimationDesc("Sprites/Chase", fw: 64, fh: 64, count: 5, time: 0.10f);
            var wanderAnim = new AnimationDesc("Sprites/Wanderer", fw: 200, fh: 200, count: 8, time: 0.12f);
            var patrolAnim = new AnimationDesc("Sprites/Patrol", fw: 80, fh: 100, count: 5, time: 0.15f);

            float speedMult = difficulty switch
            {
                StartScreen.Difficulty.Makkelijk => 0.8f,
                StartScreen.Difficulty.Moeilijk => 1.0f,
                _ => 1.0f
            };

            // basis enemies
            yield return new EnemyDefinition
            {
                AI = new ChaseAi(),
                Anim = chaserAnim,
                Position = new Vector2(300, 300),
                HitboxW = 36,
                HitboxH = 44,
                MaxSpeed = 120f * speedMult,
                Scale = 1.0f,
                DrawOffset = new Vector2(6, 18),
                TargetHeightPx = 52
            };

            yield return new EnemyDefinition
            {
                AI = new WanderAi(),
                Anim = wanderAnim,
                Position = new Vector2(900, 220),
                HitboxW = 50,
                HitboxH = 56,
                MaxSpeed = 90f * speedMult,
                Scale = 1.0f,
                DrawOffset = new Vector2(8, 22),
                TargetHeightPx = 60
            };

            yield return new EnemyDefinition
            {
                AI = new WanderAi(),
                Anim = wanderAnim,
                Position = new Vector2(200, 500),
                HitboxW = 50,
                HitboxH = 56,
                MaxSpeed = 90f * speedMult,
                Scale = 1.0f,
                DrawOffset = new Vector2(8, 22),
                TargetHeightPx = 60
            };

            yield return new EnemyDefinition
            {
                AI = new PatrolAi(new Vector2(500, 200), new Vector2(1100, 200)),
                Anim = patrolAnim,
                Position = new Vector2(500, 200),
                HitboxW = 32,
                HitboxH = 44,
                MaxSpeed = 100f * speedMult,
                Scale = 1.0f,
                DrawOffset = new Vector2(6, 20),
                TargetHeightPx = 58
            };

            // extra enemies bij Moeilijk
            if (difficulty == StartScreen.Difficulty.Moeilijk)
            {
                yield return new EnemyDefinition
                {
                    AI = new ChaseAi(),
                    Anim = chaserAnim,
                    Position = new Vector2(600, 400),
                    HitboxW = 36,
                    HitboxH = 44,
                    MaxSpeed = 120f * speedMult,
                    Scale = 1.0f,
                    DrawOffset = new Vector2(6, 18),
                    TargetHeightPx = 52
                };
                yield return new EnemyDefinition
                {
                    AI = new PatrolAi(new Vector2(200, 600), new Vector2(1100, 400)),
                    Anim = patrolAnim,
                    Position = new Vector2(500, 200),
                    HitboxW = 32,
                    HitboxH = 44,
                    MaxSpeed = 100f * speedMult,
                    Scale = 1.0f,
                    DrawOffset = new Vector2(6, 20),
                    TargetHeightPx = 58
                };

                yield return new EnemyDefinition
                {
                    AI = new WanderAi(),
                    Anim = wanderAnim,
                    Position = new Vector2(1050, 320),
                    HitboxW = 50,
                    HitboxH = 56,
                    MaxSpeed = 90f * speedMult,
                    Scale = 1.0f,
                    DrawOffset = new Vector2(8, 22),
                    TargetHeightPx = 60
                };
            }
        }

    }
}
