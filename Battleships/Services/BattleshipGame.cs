using Battleships.Interfaces;

namespace Battleships.Services
{
    public class BattleshipGame: IBattleshipGame {
        private IConsole _console { get; }

        public BattleshipGame(IConsole console)
        {
            _console = console;
        }


        public void Show()
        {
            _console.WriteLine("FAIL");
        }
    }
}