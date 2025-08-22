using Imenyaan.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Entities.Definitions
{
    public interface ILevelDefinition
    {
        string BackgroundAsset { get; }
        int VictoryGoal { get; }                          // nieuw
        IEnumerable<ObstacleDefinition> Obstacles();
        IEnumerable<EnemyDefinition> Enemies(Screens.StartScreen.Difficulty diff);
        IEnumerable<(Vector2 pos, int height, int value)> Coins();  // nieuw
    }
}
