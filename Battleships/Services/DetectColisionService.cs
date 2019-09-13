using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class DetectColisionService : IDetectColisionService
    {
        public bool IsNextShipColliding(
            List<List<BattleshipGridCell>> grid,
            BattleShip ship,
            GridCoordinate firstCell)
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

        private bool IsGuessCollidingWithShips(
            List<List<BattleshipGridCell>> grid,
            BattleShip ship,
            GridCoordinate firstCell)
        {
            for (int i = 0; i < ship.Length; ++i)
            {
                var nextLine = ship.IsVertical ? firstCell.line + i : firstCell.line;
                var nextColumn = ship.IsVertical ? firstCell.column : firstCell.column + i;

                if (grid[nextLine][nextColumn] != BattleshipGridCell.Empty)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsGuessCollidingWithBorders(
            List<List<BattleshipGridCell>> grid,
            BattleShip ship,
            GridCoordinate position)
        {
            var gridSize = grid.Count();

            if (ship.IsVertical)
            {
                if (position.line + ship.Length > gridSize)
                {
                    return true;
                }
            }
            else
            {
                if (position.column + ship.Length > gridSize)
                {
                    return true;
                }
            }

            return false;
        }
    }
}