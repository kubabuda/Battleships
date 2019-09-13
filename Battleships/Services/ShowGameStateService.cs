using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class ShowGameStateService : IShowGameState
    {
        private readonly IConvertCharService _charService; // todo Service or service
        private readonly IConfiguration _configuration;
        private readonly IConsole _console;
        Dictionary<BattleshipGridCell, char> _mappings;

        public ShowGameStateService(IConvertCharService charService,
            IConsole console,
            IConfiguration configuration)
        {
            _charService = charService;
            _console = console;
            _configuration = configuration;
            // cannot be static, configuration is injectable
            _mappings = new Dictionary<BattleshipGridCell, char>
            {
                { BattleshipGridCell.Empty, _configuration.Empty },
                { BattleshipGridCell.Ship, _configuration.Empty },
                { BattleshipGridCell.Miss, _configuration.Miss },
                { BattleshipGridCell.Hit, _configuration.Hit }
            };
        }

        

        private string _invalidInputWarning
        {
            get
            {
                char maxLetter = _charService.GetLetter(_configuration.GridSize - 1);
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
            _console.WriteLine("You already have shoot there, try something else");
        }

        public void Show(BattleshipGameState state)
        {
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(1, _configuration.GridSize))}");
            int i = 0;
            foreach (var line in state.Grid)
            {
                var lineToDisplay = string.Join(' ', line.Select(l => GetCellValueToDisplay(l)));
                _console.WriteLine($"{_charService.GetLetter(i++)} {lineToDisplay} |");
            }
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(0, _configuration.GridSize).Select(_ => "-")) } ");
        }

        // todo i think it can't be extracted to separated class
        private char GetCellValueToDisplay(BattleshipGridCell die)
        {
            return _mappings[die];
        }

    }
}