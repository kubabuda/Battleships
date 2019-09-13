using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class DetectColisionService: IDetectColisionService
    {
        public bool IsNextShipColliding(
            List<List<BattleshipGridCell>> grid, 
            BattleShip ship, 
            (int x, int y) firstCell)
        {
            try
            {
                return IsGuessCollidingWithBorders(grid, ship, firstCell)
                    | IsGuessCollidingWithShips(grid, ship, firstCell);
            }
            catch (ArgumentOutOfRangeException)
            {
                return true;
            }
        }

        public bool IsGuessCollidingWithShips(
            List<List<BattleshipGridCell>> grid, 
            BattleShip ship, 
            (int x, int y) firstCell)
        {
            for (int i = 0; i < ship.Length; ++i)
            {
                var nextX = ship.IsVertical ? firstCell.x + i : firstCell.x;
                var nextY = ship.IsVertical ? firstCell.y : firstCell.y + i;

                if(grid[nextX][nextY] != BattleshipGridCell.Empty)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsGuessCollidingWithBorders(
            List<List<BattleshipGridCell>> grid, 
            BattleShip ship, 
            (int x, int y) position)
        {
            var gridSize = grid.Count();

            if(ship.IsVertical)
            {
                if (position.x + ship.Length > gridSize)
                {
                    return true;
                }
            } 
            else
            {
                if (position.y + ship.Length > gridSize)
                {
                    return true;
                }
            }

            return false;
        }
    }
}