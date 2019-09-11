using Battleships.Interfaces;
using Battleships.Models;
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
            _gameState = InitialState();
        }

        public BattleshipGameState InitialState()
        {
            return new BattleshipGameState
            {
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
            _gameState = NextState(_gameState, guess);

            Show(_gameState);
        }

        public BattleshipGameState NextState(BattleshipGameState prevState, string guess)
        {
            var lineNo = _charSvc.GetLine(guess);
            var colNo = _charSvc.GetColumn(guess);

            var newState = new BattleshipGameState 
            {
                Grid = prevState.Grid   // shallow copy
            };
            newState.Grid[lineNo][colNo] = 'x'; // apply changes

            return newState;
        }
    }
}