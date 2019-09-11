using Battleships.Interfaces;
using Battleships.Models;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipGame: IBattleshipGame
    {
        private IConvertCharService _charSvc { get; }
        private IConfiguration _configuration { get; }
        private IConsole _console { get; }
        private IBattleshipStateBuilder _stateBuilder { get; }
        private BattleshipGameState _gameState;

        public BattleshipGame(
            IConvertCharService charSvc,
            IConfiguration configuration,
            IConsole console,
            IBattleshipStateBuilder stateBuilder)
        {
            _charSvc = charSvc;
            _configuration = configuration;
            _console = console;
            _stateBuilder = stateBuilder;
            _gameState = _stateBuilder.Build();
        }


        public void Show()
        {
            Show(_gameState);
        }

        private void Show(BattleshipGameState state)
        {
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(1, _configuration.GridSize))}");
            int i = 0;
            foreach (var line in state.Grid)
            {
                _console.WriteLine($"{_charSvc.GetLetter(i++)} {string.Join(' ', line)} |");
            }
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(0, _configuration.GridSize).Select(_ => "-")) } ");
        }

        public void Play(string guess)
        {
            _gameState = _stateBuilder.Build(_gameState, guess);

            Show(_gameState);
        }
    }
}