using Battleships.Interfaces;
using Battleships.Models;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class ShowGameStateService : IShowGameState
    {
        private readonly IConvertCharService _charSvc;
        private readonly IConfiguration _configuration;
        private readonly IConsole _console;

        public ShowGameStateService(IConvertCharService charSvc,
            IConsole console,
            IConfiguration configuration)
        {
            _charSvc = charSvc;
            _console = console;
            _configuration = configuration;
        }


        private string _invalidInputWarning
        {
            get {

                char maxLetter = _charSvc.GetLetter(_configuration.GridSize - 1);
                int maxNumber = _configuration.GridSize;
                
                return $"Invalid cell, A-{maxLetter} and 1-{maxNumber} are allowed";
            }   
        }

        public void DisplayInputWarning()
        {
            _console.WriteLine(_invalidInputWarning);
        }

        public void Show(BattleshipGameState state)
        {
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(1, _configuration.GridSize))}");
            int i = 0;
            foreach (var line in state.Grid)
            {
                var lineToDisplay = string.Join(' ', line.Select(l => GetCellValueToDisplay(l)));
                _console.WriteLine($"{_charSvc.GetLetter(i++)} {lineToDisplay} |");
            }
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(0, _configuration.GridSize).Select(_ => "-")) } ");
        }

        private char GetCellValueToDisplay(BattleshipGridCell die)
        {
            var mappings = new Dictionary<BattleshipGridCell, char>
            {
                { BattleshipGridCell.Empty, _configuration.Empty },
                { BattleshipGridCell.Miss, _configuration.Miss },
                { BattleshipGridCell.ShipHit, _configuration.Hit }
            };

            return mappings[die];
        }

    }
}