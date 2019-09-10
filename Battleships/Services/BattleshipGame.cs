using Battleships.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{

    public class BattleshipGame: IBattleshipGame {
        private const int _ASCI_A = 65;
        private const int _gridSize = 10;
        private IConsole _console { get; }
        private BattleshipGameState _gameState;

        public BattleshipGame(IConsole console)
        {
            _console = console;
            _gameState = NewGame();
        }

        public BattleshipGameState NewGame()
        {
            return new BattleshipGameState
            {
                GridSize = _gridSize,
                Grid = new List<IEnumerable<char>>(Enumerable.Range(0, _gridSize)
                    .Select(_ => new List<char>(
                        Enumerable.Range(0, _gridSize).Select(__ => ' ').ToList())
                    ).ToList())
            };
        }

        public void Show()
        {
            _console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for(int i = 0; i < _gridSize; ++i)
            {
                _console.WriteLine($"{GetLetter(i)}                     |");
            }
            _console.WriteLine("  - - - - - - - - - - ");
        }

        public char GetLetter(int i)
        {
            return Convert.ToChar(_ASCI_A + i);
        }

        public void Play(string guess)
        {
            throw new NotImplementedException();
        }

        public class BattleshipGameState {
            public int GridSize { get; set; }
            public IEnumerable<IEnumerable<char>> Grid { get; set; }
        }
    }
}