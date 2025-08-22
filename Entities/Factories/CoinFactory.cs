using Imenyaan.Entities.GameObstacles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Factories
{
    public static class CoinFactory
    {
        public static List<Coin> CreateAndLoad(ContentManager content, IEnumerable<(Vector2 pos, int height, int value)> defs)
        {
            var list = new List<Coin>();
            foreach (var (pos, height, value) in defs)
            {
                var c = new Coin("Props/Coin", pos, height, value);
                c.LoadContent(content);
                list.Add(c);
            }
            return list;
        }
    }
}
