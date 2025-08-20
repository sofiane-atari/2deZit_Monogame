using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Rendering
{
    public class Animation
    {
        private readonly Texture2D _sheet;
        private readonly int _frameWidth;
        private readonly int _frameHeight;
        private readonly int _frameCount;
        private readonly float _frameTime;

        private float _timer;
        private int _currentFrame;

        public Animation(Texture2D sheet, int frameWidth, int frameHeight, int frameCount, float frameTime)
        {
            _sheet = sheet;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _frameCount = frameCount;
            _frameTime = frameTime;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer >= _frameTime)
            {
                _timer = 0f;
                _currentFrame = (_currentFrame + 1) % _frameCount;
            }
        }

        public void Draw(SpriteBatch sb, Vector2 position, float scale, bool flipX = false)
        {
            var source = new Rectangle(_currentFrame * _frameWidth, 0, _frameWidth, _frameHeight);
            var effects = flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            sb.Draw(_sheet, position, source, Color.White, 0f, Vector2.Zero, scale, effects, 0f);
        }
    }
}
