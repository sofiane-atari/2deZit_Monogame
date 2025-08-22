using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imenyaan.Managers
{
    public class GameStateManager
    {
        private int _score;
        public int VictoryGoal { get; } = 5;

        public bool CheckVictory() => _score >= VictoryGoal;
        public void AddScore(int value) => _score += value;
        public int Score => _score;
    }
}
