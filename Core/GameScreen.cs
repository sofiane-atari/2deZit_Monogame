using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Core
{
    public abstract class GameScreen
    {
        protected ScreenManager Screens { get; private set; }
        protected Game1 Game { get; private set; }


        public virtual void Init(ScreenManager screens, Game1 game)
        {
            Screens = screens;
            Game = game;
        }


        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public virtual void Unload() { }
    }
}
