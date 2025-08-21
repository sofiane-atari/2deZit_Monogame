using Imenyaan.Screens;
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
        IEnumerable<ObstacleDefinition> Obstacles();
        IEnumerable<EnemyDefinition> Enemies(StartScreen.Difficulty difficulty);
    }
}
