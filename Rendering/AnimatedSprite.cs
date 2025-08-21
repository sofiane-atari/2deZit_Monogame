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
        private Texture2D _tex;
        private int _frameWidth, _frameHeight;
        private int _frameCount, _framesPerRow, _startFrame;
        private float _frameTime, _timer;
        private int _current; // 0..(frameCount-1)

        public void Load(ContentManager content, string asset,
                         int frameWidth, int frameHeight,
                         int frameCount, float frameTime,
                         int framesPerRow = 0, int startFrame = 0)
        {
            _tex = content.Load<Texture2D>(asset);
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _frameCount = frameCount;
            _frameTime = frameTime;
            _framesPerRow = framesPerRow <= 0 ? frameCount : framesPerRow; // strip of grid
            _startFrame = startFrame;
            _timer = 0f;
            _current = 0;
        }

        public void Update(GameTime gt)
        {
            _timer += (float)gt.ElapsedGameTime.TotalSeconds;
            if (_timer >= _frameTime)
            {
                _timer = 0f;
                _current = (_current + 1) % _frameCount;
            }
        }

        public void Draw(SpriteBatch sb, Vector2 pos, float scale = 1f, bool flipX = false, Color? tint = null)
        {
            var (src, _) = GetSourceRect();
            var effects = flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            sb.Draw(_tex, position: pos, sourceRectangle: src, color: tint ?? Color.White,
                    rotation: 0f, origin: Vector2.Zero, scale: scale, effects: effects, layerDepth: 0f);
        }

        private (Rectangle rect, int absoluteIndex) GetSourceRect()
        {
            int absolute = _startFrame + _current;
            int col = absolute % _framesPerRow;
            int row = absolute / _framesPerRow;
            var rect = new Rectangle(col * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);
            return (rect, absolute);
        }
    }
}
