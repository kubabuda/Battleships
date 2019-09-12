using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipStateBuilder: IBattleshipStateBuilder
    {
        private IConfiguration _configuration;
        private IConvertCharService _charSvc { get; }
        private IRandom _random { get; }
        
        private BattleshipGridCell[] _cellStatesAfterHit;
        
        public BattleshipStateBuilder(IConvertCharService charSvc,
            IConfiguration config,
            IRandom randomSvc)
        {
            _cellStatesAfterHit = new[] { BattleshipGridCell.Miss, BattleshipGridCell.ShipHit };
            _charSvc = charSvc;
            _configuration = config;
            _random = randomSvc;
        }

        public BattleshipGameState Build()
        {
            return new BattleshipGameState
            {
                Grid = BuildGrid()
            };
        }

        private List<List<BattleshipGridCell>> BuildGrid()
        {
            var grid =  BuildEmptyGrid();
            
            foreach(var shipLength in _configuration.Ships)
            {
                var isVertical = _random.IsNextVertical();
                var ship = (shipLength: shipLength, isVertical: isVertical);
                var firstCell = GetShipStart(grid, ship);

                BuildShip(grid, ship, firstCell);
            }

            return grid;
        }

        public (int x, int y) GetShipStart(List<List<BattleshipGridCell>> grid, (int length, bool isVertical) ship)
        {
            var guess = _random.NextCell();
            while(IsGuessCollidingWithShips(grid, ship, guess))
            {
                guess = _random.NextCell();
            }

            return guess;
        }

        public bool IsGuessCollidingWithShips(List<List<BattleshipGridCell>> grid, (int length, bool isVertical) ship, (int x, int y) firstCell)
        {
            for (int i = 0; i < ship.length; ++i)
            {
                var nextX = ship.isVertical ? firstCell.x + i : firstCell.x;
                var nextY = ship.isVertical ? firstCell.y : firstCell.y + i;

                if(grid[nextX][nextY] != BattleshipGridCell.ShipUntouched)
                {
                    return false;
                }
            }

            return true;
        }

        private void BuildShip(List<List<BattleshipGridCell>> grid, (int length, bool isVertical) ship, (int x, int y) firstCell)
        {

            for (int i = 0; i < ship.length; ++i)
            {
                var nextX = ship.isVertical ? firstCell.x + i : firstCell.x;
                var nextY = ship.isVertical ? firstCell.y : firstCell.y + i;

                grid[nextX][nextY] = BattleshipGridCell.ShipUntouched;
            }
        }

        private List<List<BattleshipGridCell>> BuildEmptyGrid()
        {
            return Enumerable.Range(0, _configuration.GridSize)
                .Select(_ => Enumerable.Range(0, _configuration.GridSize)
                    .Select(__ => BattleshipGridCell.Empty).ToList())
                .ToList();
        }

        public BattleshipGameState Build(BattleshipGameState prevState, string guess)
        {
            var lineNo = _charSvc.GetLine(guess);
            var colNo = _charSvc.GetColumn(guess);

            var newState = new BattleshipGameState
            {
                Grid = prevState.Grid   // shallow copy
            };
            newState.Grid[lineNo][colNo] = NewCellState(newState.Grid[lineNo][colNo]);

            return newState;
        }

        public BattleshipGridCell NewCellState(BattleshipGridCell prevDieState)
        {
            if (_cellStatesAfterHit.Contains(prevDieState))
            {
                throw new InvalidOperationException();
            }
            var mappings = new Dictionary<BattleshipGridCell, BattleshipGridCell>
            {
                { BattleshipGridCell.Empty, BattleshipGridCell.Miss },
                { BattleshipGridCell.ShipUntouched, BattleshipGridCell.ShipHit },
            };

            return mappings[prevDieState];
        }
    }
}