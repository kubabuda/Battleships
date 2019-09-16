using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System.Linq;

namespace Battleships.Services
{
    public class ShowGameStateService : IShowGameState
    {
        private readonly IConvertCharService _charService;
        private readonly IBattleshipsConfiguration _configuration;
        private readonly IConsole _console;
        private readonly ICellMapper _mapper;

        public ShowGameStateService(IConvertCharService charService,
            IConsole console,
            IBattleshipsConfiguration configuration,
            ICellMapper mapper)
        {
            _charService = charService;
            _console = console;
            _configuration = configuration;
            _mapper = mapper;
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
                var lineToDisplay = string.Join(' ', line.Select(l => _mapper.GetCellValueToDisplay(l)));
                _console.WriteLine($"{_charService.GetLetter(i++)} {lineToDisplay} |");
            }
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(0, _configuration.GridSize).Select(_ => "-")) } ");
        }
    }
}