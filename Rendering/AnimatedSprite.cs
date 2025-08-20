using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Rendering
{
    public class AnimatedSprite
    {
        private Animation _animation;

        public void Load(ContentManager content, string assetName, int frameWidth, int frameHeight, int frameCount, float frameTime)
        {
            var sheet = content.Load<Texture2D>(assetName);
            _animation = new Animation(sheet, frameWidth, frameHeight, frameCount, frameTime);
        }

        public void Update(GameTime gameTime) => _animation?.Update(gameTime);

        public void Draw(SpriteBatch sb, Vector2 pos, float scale, bool flipX = false) =>
            _animation?.Draw(sb, pos,scale, flipX);
    }
}
