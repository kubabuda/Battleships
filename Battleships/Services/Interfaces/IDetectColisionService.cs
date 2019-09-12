using System.Collections.Generic;
using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface IDetectColisionService
    {
        bool IsGuessColliding(List<List<BattleshipGridCell>> grid, 
            BattleShip ship, 
            (int x, int y) firstCell);
    }
}