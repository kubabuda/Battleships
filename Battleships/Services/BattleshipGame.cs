using Battleships.Interfaces;
using Battleships.Models;
using System;
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

        private readonly IShowGameState _gameShowService;
        private BattleshipGameState _gameState;

        public BattleshipGame(
            IConvertCharService charSvc,
            IConfiguration configuration,
            IConsole console,
            IBattleshipStateBuilder stateBuilder,
            IShowGameState gameShowService)
        :this(charSvc, configuration, console, stateBuilder, gameShowService, stateBuilder.Build()) { }

        public BattleshipGame(
            IConvertCharService charSvc,
            IConfiguration configuration,
            IConsole console,
            IBattleshipStateBuilder stateBuilder,
            IShowGameState gameShowService,
            BattleshipGameState gameState
        ) {
            _charSvc = charSvc;
            _configuration = configuration;
            _console = console;
            _stateBuilder = stateBuilder;
            _gameShowService = gameShowService;
            _gameState = gameState;
        }

        public void Show()
        {
            Show(_gameState);
        }

        private void Show(BattleshipGameState state)
        {
            _gameShowService.Show(state);
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

        public bool IsFinished()
        {
            return IsFinished(_gameState);
        }

        private bool IsFinished(BattleshipGameState gameState)
        {
            return !gameState.Grid.Any(
                line => line.Any(
                    cell => cell == BattleshipGridCell.ShipUntouched));
        }
    }
}