using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipGame: IBattleshipGame
    {
        private IConvertCharService _charSvc { get; }
        private IConfiguration _configuration { get; }
        private IConsole _console { get; }
        private IBattleshipStateBuilder _stateBuilder { get; }

        private string _invalidInputWarning
        {
            get {

                char maxLetter = _charSvc.GetLetter(_configuration.GridSize - 1);
                int maxNumber = _configuration.GridSize;
                
                return $"Invalid cell, A-{maxLetter} and 1-{maxNumber} are allowed";
            }   
        } 
        private BattleshipGameState _gameState;

        public BattleshipGame(
            IConvertCharService charSvc,
            IConfiguration configuration,
            IConsole console,
            IBattleshipStateBuilder stateBuilder)
        :this(charSvc, configuration, console, stateBuilder, stateBuilder.Build()) { }

        public BattleshipGame(
            IConvertCharService charSvc,
            IConfiguration configuration,
            IConsole console,
            IBattleshipStateBuilder stateBuilder, 
            BattleshipGameState gameState
        ) {
            _charSvc = charSvc;
            _configuration = configuration;
            _console = console;
            _stateBuilder = stateBuilder;
            _gameState = gameState;
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
                var lineToDisplay = string.Join(' ', line.Select(l => GetCellValueToDisplay(l)));
                _console.WriteLine($"{_charSvc.GetLetter(i++)} {lineToDisplay} |");
            }
            _console.WriteLine($"  {string.Join(' ', Enumerable.Range(0, _configuration.GridSize).Select(_ => "-")) } ");
        }

        public void Play(string guess)
        {
            
            try {
                _gameState = _stateBuilder.Build(_gameState, guess);
            
                Show(_gameState);
            }
            catch (ArgumentOutOfRangeException)
            {
                _console.WriteLine(_invalidInputWarning);
            }
            catch (InvalidOperationException)
            {
                _console.WriteLine("You already had shoot there, try something else");
            }
        }

        private char GetCellValueToDisplay(BattleshipGridCell die)
        {
            var mappings = new Dictionary<BattleshipGridCell, char>
            {
                { BattleshipGridCell.Empty, _configuration.Empty },
                { BattleshipGridCell.Miss, _configuration.Miss },
                { BattleshipGridCell.ShipHit, _configuration.Hit }
            };

            return mappings[die];
        }

        public bool IsFinished()
        {
            return _gameState.Grid.Any(l => l.Any(cell => cell == BattleshipGridCell.ShipUntouched));
        }
    }
}