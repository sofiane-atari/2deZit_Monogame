using Imenyaan.Core;
using Imenyaan.Entities;
using Imenyaan.Entities.Ai;
using Imenyaan.Entities.Definitions;
using Imenyaan.Entities.Factories;
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
        private SpriteFont _font;
        private readonly StartScreen.Difficulty _difficulty;
        private readonly ILevelDefinition _level;
        private KeyboardState _prevKb;
        private Hero _hero;
        private List<Enemy> _enemies;

        private Texture2D _background;
        private List<Obstacle> _obstacles;
        private Texture2D _pixel;
        private Rectangle _worldBounds;

        public GameplayScreen(StartScreen.Difficulty difficulty, ILevelDefinition level)
        {
            _difficulty = difficulty;
            _level = level;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            _background = content.Load<Texture2D>("Sprites/Background");

            // World bounds = viewport (of volledige world-size als je later een grotere map hebt)
            var vp = Game.GraphicsDevice.Viewport;
            _worldBounds = new Rectangle(0, 0, vp.Width, vp.Height);

            // Debug pixel
            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // Obstakels (je bestaande factory/level-definitions)
            _obstacles = ObstacleFactory.BuildAll(new Level1Definition().Obstacles(), content);

            // Hero
            _hero = new Hero();
            _hero.LoadContent(content);

            // Enemies (3 types)
            var chaserAnim = new AnimationDesc("Sprites/Chase", fw: 64, fh: 64, count: 5, time: 0.10f);
            var wanderAnim = new AnimationDesc("Sprites/Wanderer", fw: 200, fh: 200, count: 8, time: 0.12f);
            var patrolAnim = new AnimationDesc("Sprites/Patrol", fw: 80, fh: 100, count: 5, time: 0.15f);

            _enemies = new List<Enemy>
            {
                // Chaser: target hoogte 52 px
                new Enemy(new ChaseAi(), chaserAnim, new Vector2(300,300),
                          hitboxW:0, hitboxH:0, maxSpeed:100f,
                          scale:1.0f, drawOffset:new Vector2(6,18),
                          targetHeightPx: 52),

                // Wanderer: target hoogte 60 px
                new Enemy(new WanderAi(), wanderAnim, new Vector2(900,220),
                          hitboxW:0, hitboxH:0, maxSpeed: 90f,
                          scale:1.0f, drawOffset:new Vector2(8,22),
                          targetHeightPx: 60),

                // Patrol: target hoogte 58 px (of kies targetWidthPx als je breedtes wil matchen)
                new Enemy(new PatrolAi(new Vector2(500,200), new Vector2(1100,200)),
                          patrolAnim, new Vector2(500,200),
                          hitboxW:0, hitboxH:0, maxSpeed:100f,
                          scale:1.0f, drawOffset:new Vector2(6,20),
                          targetHeightPx: 58),
            };
            foreach (var e in _enemies) e.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape)) Screens.ChangeScreen(new StartScreen());

            // verzamel colliders van obstacles (als IEnumerable)
            var colliders = System.Linq.Enumerable.Select(_obstacles, o => o.Collider);

            // Enemies
            foreach (var e in _enemies)
                e.Update(gameTime, _hero.Position, colliders, _worldBounds);

            // Hero
            _hero.UpdateWithCollision(gameTime, kb, colliders, _worldBounds);

            // contact op enemy => damage
            foreach (var e in _enemies)
            {
                if (_hero.Hitbox.Intersects(e.Collider))
                {
                    bool died = _hero.TakeHit();
                    if (died)
                    {
                        Screens.ChangeScreen(new GameOverScreen(_difficulty, _level)); // nieuw screen
                        return; // stop verdere update deze frame
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var vp = Game.GraphicsDevice.Viewport;
            spriteBatch.Draw(_background, new Rectangle(0, 0, vp.Width, vp.Height), Color.White);

            foreach (var o in _obstacles)
                o.Draw(spriteBatch, debug: false, debugPixel: _pixel);

            foreach (var e in _enemies)
                e.Draw(spriteBatch);

            _hero.Draw(spriteBatch);

            spriteBatch.DrawString(_font, "ESC = menu", new Vector2(20, 20), Color.White);
            spriteBatch.DrawString(_font, $"Levens: {_hero.Lives}", new Vector2(20, 50), Color.White);
        }
    }
}