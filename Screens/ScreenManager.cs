using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Screens
{
    public class ScreenManager
    {
        private readonly Game1 _game;
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private GameScreen _current;


        public ScreenManager(Game1 game, ContentManager content, SpriteBatch spriteBatch)
        {
            _game = game;
            _content = content;
            _spriteBatch = spriteBatch;
        }


        public void ChangeScreen(GameScreen newScreen)
        {
            _current?.Unload();
            _current = newScreen;
            _current.Init(this, _game);
            _current.LoadContent(_content);
        }


        public void Update(GameTime gameTime) => _current?.Update(gameTime);


        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _current?.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();
        }
    }
}
