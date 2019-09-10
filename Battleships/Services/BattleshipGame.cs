using Battleships.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{

    public class BattleshipGame: IBattleshipGame {
        private const int _gridSize = 10;
        private IConsole _console { get; }
        private IConvertCharService _charSvc { get; }
        private BattleshipGameState _gameState;

        public BattleshipGame(IConsole console, IConvertCharService charSvc)
        {
            _charSvc = charSvc;
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
                _console.WriteLine($"{_charSvc.GetLetter(i++)} {string.Join(' ', line)} |");
            }
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(0, _gridSize).Select(_ => "-")) } ");
        }


        public void Play(string guess)
        {
            var lineNo = _charSvc.GetLine(guess);
            var colNo = _charSvc.GetColumn(guess);
            _gameState.Grid[lineNo][colNo] = 'x';
        }

        public class BattleshipGameState {
            public int GridSize { get; set; }
            public List<List<char>> Grid { get; set; }
        }
    }
}