using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class ShowGameStateService : IShowGameState
    {
        private readonly IConvertCharService _charSvc; // todo svc or service
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
            get
            {
                char maxLetter = _charSvc.GetLetter(_configuration.GridSize - 1);
                int maxNumber = _configuration.GridSize;

                return $"Invalid cell, A-{maxLetter} and 1-{maxNumber} are allowed";
            }
        }

        public void DisplayInputWarning()
        {
            _console.WriteLine(_invalidInputWarning);
        }

        public void DisplayRetryWarning()
        {
            _console.WriteLine("You already had shoot there, try something else"); // todo english motherfucker, do you speak it?
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

// todo i think it can't be extracted to separated class
        private char GetCellValueToDisplay(BattleshipGridCell die)
        {
            // todo can be static
            var mappings = new Dictionary<BattleshipGridCell, char>
            {
                { BattleshipGridCell.Empty, _configuration.Empty },
                { BattleshipGridCell.Ship, _configuration.Empty },
                { BattleshipGridCell.Miss, _configuration.Miss },
                { BattleshipGridCell.Hit, _configuration.Hit }
            };

            return mappings[die];
        }

    }
}