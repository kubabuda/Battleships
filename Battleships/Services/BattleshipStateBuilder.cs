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
        private IDetectColisionService _detectCollisionService;
        private IRandom _random { get; }
        
        private BattleshipGridCell[] _cellStatesAfterHit;
        
        public BattleshipStateBuilder(IConvertCharService charSvc,
            IConfiguration config,
            IDetectColisionService detectCollisionService,
            IRandom randomSvc)
        {
            _cellStatesAfterHit = new[] { BattleshipGridCell.Miss, BattleshipGridCell.ShipHit };
            _charSvc = charSvc;
            _configuration = config;
            _detectCollisionService = detectCollisionService;
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
            
            foreach(var shipLength in _configuration.Ships.OrderByDescending(s => s))
            {
                var isVertical = _random.IsNextVertical();
                var ship = BuildShip(shipLength, isVertical);
                var firstCell = GetShipStart(grid, ship);

                PlaceShipOnGrid(grid, ship, firstCell);
            }

            return grid;
        }

        private static BattleShip BuildShip(int shipLength, bool isVertical)
        {
            return new BattleShip { length = shipLength, isVertical = isVertical };
        }

        public (int x, int y) GetShipStart(List<List<BattleshipGridCell>> grid, BattleShip ship)
        {
            var guess = GetRandomGridCell();

            while (_detectCollisionService.IsGuessColliding(grid, ship, guess))
            {
                guess = GetRandomGridCell();
            }

            return guess;
        }

        private (int x, int y) GetRandomGridCell()
        {
            return _random.NextCell();
        }

        private void PlaceShipOnGrid(List<List<BattleshipGridCell>> grid, BattleShip ship, (int x, int y) firstCell)
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