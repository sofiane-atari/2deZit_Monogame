using Imenyaan.Entities;
using Imenyaan.Entities.Ai;
using Imenyaan.Entities.Controller;
using Imenyaan.Entities.Definitions;
using Imenyaan.Entities.Factories;
using Imenyaan.Entities.GameObstacles;
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

        private Hero _hero1;           
        private Hero _hero2;           
        private List<Enemy> _enemies;
        private SimpleSprite _heart;

        private Texture2D _background;
        private List<Obstacle> _obstacles;
        private Texture2D _pixel;
        private Rectangle _worldBounds;

        private List<Coin> _coins;
        private int _score;
        private const int VictoryScore = 5;

        public GameplayScreen(StartScreen.Difficulty difficulty, ILevelDefinition level)
        {
            _difficulty = difficulty;
            _level = level;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            _background = content.Load<Texture2D>(_level.BackgroundAsset);

            // World bounds 
            var vp = Game.GraphicsDevice.Viewport;
            _worldBounds = new Rectangle(0, 0, vp.Width, vp.Height);

            // Debug pixel
            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // Obstakels via level + factory
            _obstacles = ObstacleFactory.BuildAll(_level.Obstacles(), content);

            // Hero
            var hero1Cfg = HeroConfig.ForKeyboard(
                    InputControllerFactory.CreateArrows(),
                    "Sprites/hero_walk", 322, 373, 3, 0.12f, 3)
                with
            {
                StartPos = new Vector2(200, 200),
                Scale = 0.15f,
                HitboxW = 40,
                HitboxH = 50,
                InvulDuration = 1f,
            };

            var hero2Cfg = HeroConfig.ForKeyboard(
                    InputControllerFactory.CreateWASD(),
                    "Sprites/Hero2", 64, 66, 8, 0.10f, 8)
                with
            {
                StartPos = new Vector2(200, 300),
                TargetHeightPx = 50,
                HitboxW = 40,
                HitboxH = 50,
                InvulDuration = 1f,
            };

            _hero1 = new Hero(hero1Cfg);
            _hero2 = new Hero(hero2Cfg);
            _hero1.LoadContent(content);
            _hero2.LoadContent(content);

            // Enemies via level-definition
            _enemies = _level.Enemies(_difficulty)
                             .Select(def =>
                             {
                                 var e = new Enemy(
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
                                 e.LoadContent(content);
                                 return e;
                             })
                             .ToList();

            _heart = new SimpleSprite();
            _heart.Load(content, "Props/Heart");
            
            float heartHeight = 20f;
            float s = heartHeight / _heart.SizePixels.Y;
            _heart.Scale = new Vector2(s, s);


            _coins = new List<Coin>
            {
                new Coin("Props/Coin", new Vector2(200, 120), targetHeightPx: 28, value: 1),
                new Coin("Props/Coin", new Vector2(420, 180), targetHeightPx: 28, value: 1),
                new Coin("Props/Coin", new Vector2(760, 260), targetHeightPx: 28, value: 1),
                new Coin("Props/Coin", new Vector2(980, 460), targetHeightPx: 28, value: 1),
                new Coin("Props/Coin", new Vector2(300, 600), targetHeightPx: 28, value: 1),
            };
            foreach (var c in _coins) c.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape))
            {
                Screens.ChangeScreen(new StartScreen());
                return;
            }

            var colliders = _obstacles.Select(o => o.Collider);

            // Enemies targetten dichtstbijzijnde hero
            foreach (var e in _enemies)
            {
                var targetPos = NearestHeroPosition(e, _hero1, _hero2);
                e.Update(gameTime, targetPos, colliders, _worldBounds);
            }

            // Beiden updaten
            _hero1.UpdateWithCollision(gameTime, colliders, _worldBounds);
            _hero2.UpdateWithCollision(gameTime, colliders, _worldBounds);

            // Coins: pickup door eender welke hero
            foreach (var c in _coins)
            {
                if (!c.Collected && (c.TryCollect(_hero1.Hitbox) || c.TryCollect(_hero2.Hitbox)))
                {
                    _score += c.Value;
                    if (_score >= VictoryScore)
                    {
                        Screens.ChangeScreen(new VictoryScreen());
                        return;
                    }
                }
            }

            // Enemy contact → damage voor elk
            foreach (var e in _enemies)
            {
                if (_hero1.Hitbox.Intersects(e.Collider))
                {
                    if (_hero1.TakeHit())
                    {
                        Screens.ChangeScreen(new GameOverScreen(_difficulty, _level));
                        return;
                    }
                }
                if (_hero2.Hitbox.Intersects(e.Collider))
                {
                    if (_hero2.TakeHit())
                    {
                        Screens.ChangeScreen(new GameOverScreen(_difficulty, _level));
                        return;
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

            // Coins voor of na heroes, zoals je mooi vindt
            foreach (var c in _coins)
                c.Draw(spriteBatch);

            _hero1.Draw(spriteBatch);
            _hero2.Draw(spriteBatch);

            // HUD
            spriteBatch.DrawString(_font, "ESC = menu", new Vector2(20, 40), Color.White);
            spriteBatch.DrawString(_font, $"Score: {_score}/{VictoryScore}", new Vector2(20, 100), Color.White);

            // P1 (bovenste rij)
            var hudPos1 = new Vector2(20, 10);
            int spacing = (int)(_heart.SizeOnScreen.X + 6);
            for (int i = 0; i < _hero1.Lives; i++)
                _heart.Draw(spriteBatch, hudPos1 + new Vector2(i * spacing, 0));

            // P2 (tweede rij)
            var hudPos2 = new Vector2(20, 10 + _heart.SizeOnScreen.Y + 6);
            for (int i = 0; i < _hero2.Lives; i++)
                _heart.Draw(spriteBatch, hudPos2 + new Vector2(i * spacing, 0));
        }

        private static Vector2 NearestHeroPosition(Enemy enemy, Hero h1, Hero h2)
        {
            float d1 = Vector2.DistanceSquared(enemy.Position, h1.Position);
            float d2 = Vector2.DistanceSquared(enemy.Position, h2.Position);
            return d1 <= d2 ? h1.Position : h2.Position;
        }
    }
}