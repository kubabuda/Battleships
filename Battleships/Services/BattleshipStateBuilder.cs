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
        
        public BattleshipStateBuilder(IReadUserGuess guessReadSvc,
            IConfiguration config,
            IDetectColisionService detectCollisionService,
            IRandom randomSvc)
        {
            _cellStatesAfterHit = new[] { BattleshipGridCell.Miss, BattleshipGridCell.Hit };
            _guessReader = guessReadSvc;
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
            return new BattleShip { Length = shipLength, IsVertical = isVertical };
        }

        public (int x, int y) GetShipStart(List<List<BattleshipGridCell>> grid, BattleShip ship)
        {
            var nextShipStart = GetRandomGridCell();

            while (_detectCollisionService.IsNextShipColliding(grid, ship, nextShipStart))
            {
                nextShipStart = GetRandomGridCell();
            }

            return nextShipStart;
        }

        private (int x, int y) GetRandomGridCell()
        {
            return _random.NextCell();
        }

        private void PlaceShipOnGrid(List<List<BattleshipGridCell>> grid, BattleShip ship, (int x, int y) firstCell)
        {

            for (int i = 0; i < ship.Length; ++i)
            {
                var nextX = ship.IsVertical ? firstCell.x + i : firstCell.x;
                var nextY = ship.IsVertical ? firstCell.y : firstCell.y + i;

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
            var g = _guessReader.GetCordinates(guess);

            var newState = new BattleshipGameState
            {
                Grid = prevState.Grid   // shallow copy
            };
            newState.Grid[g.line][g.column] = NewCellState(newState.Grid[g.line][g.column]);

            return newState;
        }


        public BattleshipGridCell NewCellState(BattleshipGridCell prevDieState)
        {
            if (_cellStatesAfterHit.Contains(prevDieState))
            {
                throw new CellRepetitionException();
            }
            var mappings = new Dictionary<BattleshipGridCell, BattleshipGridCell>
            {
                { BattleshipGridCell.Empty, BattleshipGridCell.Miss },
                { BattleshipGridCell.Ship, BattleshipGridCell.Hit },
            };

            return mappings[prevDieState];
        }
    }
}