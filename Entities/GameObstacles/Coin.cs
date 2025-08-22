using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.GameObstacles
{
    public class Coin : IGameEntity
    {
        public Vector2 Position { get; private set; }
        public Rectangle Collider { get; private set; }
        public bool Collected { get; private set; }
        public int Value { get; }

        private readonly string _asset;
        private readonly int _targetHeightPx;
        private SimpleSprite _sprite;

        public Coin(string asset, Vector2 pos, int targetHeightPx = 28, int value = 1)
        {
            _asset = asset;
            Position = pos;
            _targetHeightPx = targetHeightPx;
            Value = value;
        }

        public void LoadContent(ContentManager content)
        {
            _sprite = new SimpleSprite();
            _sprite.Load(content, _asset);

            var tex = _sprite.SizePixels;
            float s = (tex.Y > 0 && _targetHeightPx > 0) ? _targetHeightPx / (float)tex.Y : 1f;
            _sprite.Scale = new Vector2(s, s);

            var onScreen = _sprite.SizeOnScreen;
            Collider = new Rectangle((int)Position.X, (int)Position.Y, onScreen.X, onScreen.Y);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!Collected) _sprite.Draw(sb, Position);
        }

        public bool TryCollect(Rectangle heroHitbox)
        {
            if (Collected) return false;
            if (!heroHitbox.Intersects(Collider)) return false;
            Collected = true;
            return true;
        }

        // IGameEntity
        void IGameEntity.LoadContent(ContentManager content) => LoadContent(content);
        void IGameEntity.Draw(SpriteBatch sb) => Draw(sb);
    }
}
