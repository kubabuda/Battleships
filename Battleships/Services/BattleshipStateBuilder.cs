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
        
        public BattleshipStateBuilder(IConvertCharService charSvc, IConfiguration config)
        {
            _charSvc = charSvc;
            _configuration = config;
        }

        public BattleshipGameState Build()
        {
            return new BattleshipGameState
            {
                Grid = new List<List<BattleshipGridCell>>(Enumerable.Range(0, _configuration.GridSize)
                    .Select(_ => new List<BattleshipGridCell>(
                        Enumerable.Range(0,  _configuration.GridSize).Select(__ => BattleshipGridCell.Empty).ToList())
                    ).ToList())
            };
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
            var mappings = new Dictionary<BattleshipGridCell, BattleshipGridCell>
            {
                { BattleshipGridCell.Empty, BattleshipGridCell.Miss },
                { BattleshipGridCell.ShipUntouched, BattleshipGridCell.ShipHit },
            };

            return mappings[prevDieState];
        }
    }
}