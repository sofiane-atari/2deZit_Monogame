using Imenyaan.Core;
using Imenyaan.Entities;
using Imenyaan.Entities.Definitions;
using Imenyaan.Entities.Factories;
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

        private Texture2D _background;
        private List<Obstacle> _obstacles;
        private Texture2D _pixel;

        public GameplayScreen(StartScreen.Difficulty difficulty, ILevelDefinition level)
        {
            _difficulty = difficulty;
            _level = level;
        }

        public override void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/Default");
            _background = content.Load<Texture2D>(_level.BackgroundAsset);

            _pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            // Data -> objecten
            _obstacles = ObstacleFactory.BuildAll(_level.Obstacles(), content);

            _hero = new Hero();
            _hero.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            var kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Escape)) Screens.ChangeScreen(new StartScreen());

            // colliders doorgeven:
            _hero.UpdateWithCollision(gameTime, kb, _obstacles.Select(o => o.Collider));
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            // Achtergrond schermvullend
            var vp = Game.GraphicsDevice.Viewport;
            sb.Draw(_background, destinationRectangle: new Rectangle(0, 0, vp.Width, vp.Height), Color.White);

            // Obstakels (tip: sorteer op Y voor “painter’s” dieptegevoel)
            foreach (var o in _obstacles.OrderBy(x => x.Position.Y))
                o.Draw(sb, debug: false, debugPixel: _pixel); // zet debug:true om colliders transparant te zien

            _hero.Draw(sb);

            sb.DrawString(_font, "ESC = menu", new Vector2(20, 20), Color.White);
        }
    }
}