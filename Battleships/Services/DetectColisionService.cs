using Battleships.Interfaces;
using Battleships.Models;
using System.Collections.Generic;

namespace Battleships.Services
{
    public class DetectColisionService: IDetectColisionService
    {
        public bool IsGuessColliding(List<List<BattleshipGridCell>> grid, 
            BattleShip ship, 
            (int x, int y) firstCell)
        {
            return IsGuessCollidingWithShips(grid, ship, firstCell);
        }

        public bool IsGuessCollidingWithShips(List<List<BattleshipGridCell>> grid, 
            BattleShip ship, 
            (int x, int y) firstCell)
        {
            for (int i = 0; i < ship.length; ++i)
            {
                var nextX = ship.isVertical ? firstCell.x + i : firstCell.x;
                var nextY = ship.isVertical ? firstCell.y : firstCell.y + i;

                if(grid[nextX][nextY] != BattleshipGridCell.Empty)
                {
                    return true;
                }
            }

            return false;
        }
    }
}