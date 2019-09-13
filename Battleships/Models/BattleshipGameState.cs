using System.Collections.Generic;

namespace Battleships.Models
{
    public class BattleshipGameState
    {
        public List<List<BattleshipGridCell>> Grid { get; set; }
    }

    // todo add factory method
}