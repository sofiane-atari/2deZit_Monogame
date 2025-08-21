using Imenyaan.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities
{
    public class Coin
    {
        public Vector2 Position;              
        public Rectangle Collider => new Rectangle((int)Position.X, (int)Position.Y, _colliderW, _colliderH);
        public bool Collected { get; private set; }

        private readonly string _asset;
        private readonly int _targetHeightPx; 
        private int _colliderW, _colliderH;
        private readonly int _value;

        private SimpleSprite _sprite;

        public int Value => _value;

        public Coin(string asset, Vector2 pos, int targetHeightPx = 28, int value = 1)
        {
            _asset = asset;
            Position = pos;
            _targetHeightPx = targetHeightPx;
            _value = value;
        }

        public void LoadContent(ContentManager content)
        {
            _sprite = new SimpleSprite();
            _sprite.Load(content, _asset);

            // schaal de sprite naar gewenste hoogte
            var texSize = _sprite.SizePixels; // originele px
            float scale = _targetHeightPx > 0 && texSize.Y > 0
                ? _targetHeightPx / (float)texSize.Y
                : 1f;

            _sprite.Scale = new Vector2(scale, scale);

            // collider ≈ sprite-grootte
            _colliderW = (int)(texSize.X * scale);
            _colliderH = (int)(texSize.Y * scale);
        }

        public void Draw(SpriteBatch sb)
        {
            if (!Collected)
                _sprite.Draw(sb, Position);
        }

        public bool TryCollect(Rectangle heroHitbox)
        {
            if (Collected) return false;
            if (heroHitbox.Intersects(Collider))
            {
                Collected = true;
                return true;
            }
            return false;
        }
    }
}
