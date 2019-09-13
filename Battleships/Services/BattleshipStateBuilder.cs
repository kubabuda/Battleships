using Battleships.Configurations;
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
        private IReadUserGuess _guessReader;
        private IDetectColisionService _detectCollisionService;
        private IRandom _random;
        
        private BattleshipGridCell[] _cellStatesAfterHit;
        
        public BattleshipStateBuilder(IReadUserGuess guessReader,
            IConfiguration config,
            IDetectColisionService detectCollisionService,
            IRandom randomSvc)
        {
            _cellStatesAfterHit = new[] { BattleshipGridCell.Miss, BattleshipGridCell.Hit };
            _guessReader = guessReader;
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
                var ship = new BattleShip(shipLength, isVertical);
                var firstCell = GetShipStart(grid, ship);

                PlaceShipOnGrid(grid, ship, firstCell);
            }

            return grid;
        }

        public GridCoordinate GetShipStart(List<List<BattleshipGridCell>> grid, BattleShip ship)
        {
            var nextShipStart = GetRandomGridCell();

            while (_detectCollisionService.IsNextShipColliding(grid, ship, nextShipStart))
            {
                nextShipStart = GetRandomGridCell();
            }

            return nextShipStart;
        }

        private GridCoordinate GetRandomGridCell()
        {
            return _random.NextCell();
        }

        private void PlaceShipOnGrid(List<List<BattleshipGridCell>> grid, BattleShip ship, GridCoordinate firstCell)
        {
            for (int i = 0; i < ship.Length; ++i)
            {
                var nextX = ship.IsVertical ? firstCell.line + i : firstCell.line;
                var nextY = ship.IsVertical ? firstCell.line : firstCell.column + i;

                grid[nextX][nextY] = BattleshipGridCell.Ship;
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
            var guessCell = _guessReader.GetCordinates(guess);

            // state shallow copy
            // that it might be a problem with multiple threads but its easier to implement and has less overhead
            var newState = new BattleshipGameState
            {
                Grid = prevState.Grid
            };
            newState.Grid[guessCell.line][guessCell.column] = NewCellState(newState.Grid[guessCell.line][guessCell.column]);

            return newState;
        }

        private static Dictionary<BattleshipGridCell, BattleshipGridCell> _nextStateMappings = new Dictionary<BattleshipGridCell, BattleshipGridCell>
        {
            { BattleshipGridCell.Empty, BattleshipGridCell.Miss },
            { BattleshipGridCell.Ship, BattleshipGridCell.Hit },
        };
        
// todo it might be not builder responsability
        public BattleshipGridCell NewCellState(BattleshipGridCell prevCellState)
        {
            if (_cellStatesAfterHit.Contains(prevCellState))
            {
                throw new CellRepetitionException();
            }

            return _nextStateMappings[prevCellState];
        }
    }
}