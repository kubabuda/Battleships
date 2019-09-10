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
                Grid = new List<List<char>>(Enumerable.Range(0, _gridSize)
                    .Select(_ => new List<char>(
                        Enumerable.Range(0, _gridSize).Select(__ => ' ').ToList())
                    ).ToList())
            };
        }

        public void Show()
        {
            Show(_gameState);
        }

        private void Show(BattleshipGameState state)
        {
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(1, _gridSize))}");
            int i = 0;
            foreach (var line in state.Grid)
            {
                _console.WriteLine($"{GetLetter(i++)} {string.Join(' ', line)} |");
            }
            _console.WriteLine($" {string.Join(' ', Enumerable.Range(0, _gridSize)).Select(_ => '-')} ");
        }

        public char GetLetter(int i)
        {
            return Convert.ToChar(_ASCI_A + i);
        }

        public void Play(string guess)
        {
            var lineNo = Convert.ToInt32(guess[0]);
            var colNo = guess[1];
            _gameState.Grid[lineNo][colNo] = 'x';
        }

        public int GetLine(string guess) {
            throw new NotImplementedException();
        }
        public int GetColumn(string guess) {
            throw new NotImplementedException();
        }

        public class BattleshipGameState {
            public int GridSize { get; set; }
            public List<List<char>> Grid { get; set; }
        }
    }
}