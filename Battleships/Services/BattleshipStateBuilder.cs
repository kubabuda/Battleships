using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipStateBuilder: IBattleshipStateBuilder
    {
        private IConfiguration _configuration;
        private IReadUserGuess _guessReader;
        private IDetectColisionService _detectCollisionService;
        private ICellMapper _mapper;
        private IRandom _random;
        
        public BattleshipStateBuilder(IReadUserGuess guessReader,
            IConfiguration configuration,
            IDetectColisionService detectCollisionService,
            ICellMapper mapper,
            IRandom randomService)
        {
            _guessReader = guessReader;
            _configuration = configuration;
            _detectCollisionService = detectCollisionService;
            _mapper = mapper;
            _random = randomService;
        }

        public BattleshipGameState Build()
        {
            var result = BattleshipGameState.Empty(_configuration.GridSize);
            
            foreach(var shipLength in _configuration.Ships.OrderByDescending(s => s))
            {
                var isVertical = _random.IsNextVertical();
                var ship = new BattleShip(shipLength, isVertical);
                var firstCell = GetShipStart(result.Grid, ship);

                PlaceShipOnGrid(result.Grid, ship, firstCell);
            }

            return result;
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
                var nextLine = ship.IsVertical ? firstCell.Line + i : firstCell.Line;
                var nextColumn = ship.IsVertical ? firstCell.Column : firstCell.Column + i;

                grid[nextLine][nextColumn] = BattleshipGridCell.Ship;
            }
        }

        public BattleshipGameState Build(BattleshipGameState prevState, string guess)
        {
            var guessCell = _guessReader.GetCordinates(guess);

            // state shallow copy
            // that it might be a problem with multiple threads but its easier to implement and has less overhead
            var newState = new BattleshipGameState()
            {
                Grid = prevState.Grid
            };
            var newCellState = _mapper.NewCellState(newState.Grid[guessCell.Line][guessCell.Column]);
            newState.Grid[guessCell.Line][guessCell.Column] = newCellState;

            return newState;
        }
    }
}