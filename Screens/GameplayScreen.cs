using Imenyaan.Entities;
using Imenyaan.Entities.Ai;
using Imenyaan.Entities.Controller;
using Imenyaan.Entities.Definitions;
using Imenyaan.Entities.Factories;
using Imenyaan.Entities.GameObstacles;
using Imenyaan.Managers;
using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Imenyaan.Screens
{
    public class GameplayScreen : GameScreen
    {
        private readonly StartScreen.Difficulty _difficulty;
        private readonly ILevelDefinition _level;

        // Graphics
        private SpriteFont _font;
        private Texture2D _background;
        private Texture2D _pixel;
        private Rectangle _worldBounds;

        // Entities
        private Hero _hero1;
        private Hero _hero2;
        private List<Enemy> _enemies;
        private List<Obstacle> _obstacles;
        private List<Coin> _coins;

        // Managers (SRP)
        private GameStateManager _gameState;       
        private CollisionManager _collisionManager;
        private EnemyManager _enemyManager;
        private HUDRenderer _hudRenderer;          // tekent HUD met hearts/score

        public GameplayScreen(StartScreen.Difficulty difficulty, ILevelDefinition level)
        {
            _difficulty = difficulty;
            _level = level;
        }

        public override void LoadContent(ContentManager content)
        {
            LoadGraphics(content);
            LoadWorldBounds();
            LoadEntities(content);
            InitializeManagers(content); 
        }

        private void LoadGraphics(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            _background = content.Load<Texture2D>(_level.BackgroundAsset);

            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        private void LoadWorldBounds()
        {
            var vp = Game.GraphicsDevice.Viewport;
            _worldBounds = new Rectangle(0, 0, vp.Width, vp.Height);
        }

        private void LoadEntities(ContentManager content)
        {
            // Obstakels via level + factory
            _obstacles = ObstacleFactory.BuildAll(_level.Obstacles(), content);

            // Heroes (encapsulated input via controllers)
            _hero1 = CreateHero(
                content,
                InputControllerFactory.CreateArrows(),
                sprite: "Sprites/hero_walk",
                fw: 322, fh: 373, count: 3, ft: 0.12f, fpr: 3,
                startPos: new Vector2(200, 200),
                scale: 0.15f,
                targetHeight: null
            );

            _hero2 = CreateHero(
                content,
                InputControllerFactory.CreateWASD(),
                sprite: "Sprites/Hero2",
                fw: 64, fh: 66, count: 8, ft: 0.10f, fpr: 8,
                startPos: new Vector2(200, 300),
                scale: 1.0f,
                targetHeight: 50 // automatisch schalen op hoogte
            );

            // Enemies via level-definition
            _enemies = _level.Enemies(_difficulty)
                             .Select(def => CreateEnemy(def, content))
                             .ToList();

            
            _coins = new List<Coin>
            {
                new Coin("Props/Coin", new Vector2(200, 120), 28, 1),
                new Coin("Props/Coin", new Vector2(420, 180), 28, 1),
                new Coin("Props/Coin", new Vector2(760, 260), 28, 1),
                new Coin("Props/Coin", new Vector2(980, 460), 28, 1),
                new Coin("Props/Coin", new Vector2(300, 600), 28, 1),
            };
            _coins.ForEach(c => c.LoadContent(content));
        }

        private void InitializeManagers(ContentManager content)
        {
            _gameState = new GameStateManager();
            _collisionManager = new CollisionManager();

            _enemyManager = new EnemyManager(_enemies);

            // HUDRenderer heeft het font + heart sprite + heroes + victoryGoal nodig
            var heart = CreateHeartSprite(content);
            _hudRenderer = new HUDRenderer(_font, heart, _hero1, _hero2, _gameState.VictoryGoal);
        }

        private Hero CreateHero(
            ContentManager content,
            IInputController controller,
            string sprite, int fw, int fh, int count, float ft, int fpr,
            Vector2 startPos, float scale, int? targetHeight)
        {
            var cfg = HeroConfig.ForKeyboard(controller, sprite, fw, fh, count, ft, fpr) with
            {
                StartPos = startPos,
                Scale = scale,
                HitboxW = 40,
                HitboxH = 50,
                InvulDuration = 1f,
                TargetHeightPx = targetHeight ?? 0
            };

            var hero = new Hero(cfg);
            hero.LoadContent(content);
            return hero;
        }

        private Enemy CreateEnemy(EnemyDefinition def, ContentManager content)
        {
            var enemy = new Enemy(
                ai: def.AI,
                animDesc: def.Anim,
                startPos: def.Position,
                hitboxW: def.HitboxW,
                hitboxH: def.HitboxH,
                maxSpeed: def.MaxSpeed,
                scale: def.Scale,
                drawOffset: def.DrawOffset,
                targetHeightPx: def.TargetHeightPx
            );
            enemy.LoadContent(content);
            return enemy;
        }

        private SimpleSprite CreateHeartSprite(ContentManager content)
        {
            var heart = new SimpleSprite();
            heart.Load(content, "Props/Heart");
            float s = 20f / heart.SizePixels.Y; // schaal op 20px hoog
            heart.Scale = new Vector2(s, s);
            return heart;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Screens.ChangeScreen(new StartScreen());
                return;
            }

            // Colliders van obstacles
            var colliders = _obstacles.Select(o => o.Collider);

            // Enemies: laten zelf target kiezen (dichtstbijzijnde van beide heroes)
            _enemyManager.UpdateEnemies(gameTime, new[] { _hero1, _hero2 }, colliders, _worldBounds);

            // Heroes bewegen met collision + world clamp
            _hero1.UpdateWithCollision(gameTime, colliders, _worldBounds);
            _hero2.UpdateWithCollision(gameTime, colliders, _worldBounds);

            // Coins & victory
            if (CheckCoinCollisions()) return;

            // Damage & defeat
            if (CheckEnemyCollisions()) return;
        }

        private bool CheckCoinCollisions()
        {
            foreach (var coin in _coins.Where(c => !c.Collected))
            {
                if (_collisionManager.CheckCoinCollection(coin, _hero1, _hero2))
                {
                    _gameState.AddScore(coin.Value);
                    if (_gameState.CheckVictory())
                    {
                        Screens.ChangeScreen(new VictoryScreen());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckEnemyCollisions()
        {
            foreach (var enemy in _enemies)
            {
                if (_collisionManager.CheckEnemyCollision(enemy, _hero1, _hero2))
                {
                    Screens.ChangeScreen(new GameOverScreen(_difficulty, _level));
                    return true;
                }
            }
            return false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Background
            var vp = Game.GraphicsDevice.Viewport;
            spriteBatch.Draw(_background, new Rectangle(0, 0, vp.Width, vp.Height), Color.White);

            // Obstacles
            foreach (var o in _obstacles)
                o.Draw(spriteBatch, debug: false, debugPixel: _pixel);

            // Enemies
            _enemyManager.DrawEnemies(spriteBatch);

            // Coins
            foreach (var c in _coins)
                c.Draw(spriteBatch);

            // Heroes
            _hero1.Draw(spriteBatch);
            _hero2.Draw(spriteBatch);

            // HUD
            _hudRenderer.Draw(spriteBatch, _gameState.Score);
        }
    }
}