using Battleships.Interfaces;
using System;

namespace Battleships.Services
{
    public class BattleshipGame: IBattleshipGame {
        private const int ASCI_A = 65;

        private IConsole _console { get; }
        private int _gridSize => 10;

        public BattleshipGame(IConsole console)
        {
            _console = console;
        }


        public void Show()
        {
            _console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for(int i = 0; i < _gridSize; ++i) {
                _console.WriteLine($"{Convert.ToChar(ASCI_A + i)}                     |");
            }
            _console.WriteLine("  - - - - - - - - - - ");
        }

        public void Play(string guess)
        {
            throw new NotImplementedException();
        }
    }
}