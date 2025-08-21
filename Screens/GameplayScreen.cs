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
            _hero = new Hero();
            _hero.LoadContent(content);

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
            if (kb.IsKeyDown(Keys.Escape)) Screens.ChangeScreen(new StartScreen());

            // verzamel colliders van obstacles (als IEnumerable)
            var colliders = System.Linq.Enumerable.Select(_obstacles, o => o.Collider);

            // Enemies
            foreach (var e in _enemies)
                e.Update(gameTime, _hero.Position, colliders, _worldBounds);

            // Hero
            _hero.UpdateWithCollision(gameTime, kb, colliders, _worldBounds);

            foreach (var c in _coins)
            {
                if (!c.Collected && c.TryCollect(_hero.Hitbox))
                {
                    _score += c.Value;
                    if (_score >= VictoryScore)
                    {
                        Screens.ChangeScreen(new VictoryScreen());
                        return;
                    }
                }
            }

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

            foreach (var c in _coins)
                c.Draw(spriteBatch);

            // HUD score
            spriteBatch.DrawString(_font, $"Score: {_score}/{VictoryScore}", new Vector2(20, 80), Color.White);
        }
    }
}