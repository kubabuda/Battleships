using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class CellMapper : ICellMapper
    {
        private readonly BattleshipGridCell[] _cellStatesAfterHit;
        private readonly IBattleshipsConfiguration _configuration;
        private readonly Dictionary<BattleshipGridCell, char> _displayMappings;

        public CellMapper(IBattleshipsConfiguration configuration)
        {
            _cellStatesAfterHit = new[] { BattleshipGridCell.Miss, BattleshipGridCell.Hit };
            _configuration = configuration;

            // cannot be static, depend on configuration which is injectable
            _displayMappings = new Dictionary<BattleshipGridCell, char>
            {
                { BattleshipGridCell.Empty, _configuration.Empty },
                { BattleshipGridCell.Ship, _configuration.Empty },
                { BattleshipGridCell.Miss, _configuration.Miss },
                { BattleshipGridCell.Hit, _configuration.Hit }
            };
        }

        private static readonly Dictionary<BattleshipGridCell, BattleshipGridCell> _nextStateMappings = new Dictionary<BattleshipGridCell, BattleshipGridCell>
        {
            { BattleshipGridCell.Empty, BattleshipGridCell.Miss },
            { BattleshipGridCell.Ship, BattleshipGridCell.Hit },
        };

        public BattleshipGridCell NewCellState(BattleshipGridCell prevCellState)
        {
            if (_cellStatesAfterHit.Contains(prevCellState))
            {
                throw new CellRepetitionException();
            }

            return _nextStateMappings[prevCellState];
        }

        public char GetCellValueToDisplay(BattleshipGridCell cell)
        {
            return _displayMappings[cell];
        }
    }
}