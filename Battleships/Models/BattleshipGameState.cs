using System.Collections.Generic;
using System.Linq;

namespace Battleships.Models
{
    public class BattleshipGameState
    {
        public List<List<BattleshipGridCell>> Grid { get; set; }

        public static BattleshipGameState Empty(int gridSize)
        {
            return new BattleshipGameState
            {
                Grid = Enumerable.Range(0, gridSize)
                    .Select(_ => Enumerable.Range(0, gridSize)
                        .Select(__ => BattleshipGridCell.Empty).ToList())
                    .ToList()
            };
        }
    }
}