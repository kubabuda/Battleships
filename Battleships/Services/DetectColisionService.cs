using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool IsGuessCollidingWithBorders(List<List<BattleshipGridCell>> grid, BattleShip ship, (int x, int y) position)
        {
            var gridSize = grid.Count();

            if(ship.isVertical)
            {
                if (position.x + ship.length > gridSize)
                {
                    return true;
                }
            } 
            else
            {
                if (position.y + ship.length > gridSize)
                {
                    return true;
                }
            }

            return false;
        }
    }
}