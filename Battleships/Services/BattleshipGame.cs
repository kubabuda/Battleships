using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipGame: IBattleshipGame
    {
        private readonly IConsole _console;
        private readonly IBattleshipStateBuilder _stateBuilder;
        private readonly IShowGameState _gameShowService;
        private BattleshipGameState _gameState;

        public BattleshipGame(
            IConsole console,
            IBattleshipStateBuilder stateBuilder,
            IShowGameState gameShowService)
        :this(console, stateBuilder, gameShowService, stateBuilder.Build())
        { }

        public BattleshipGame(
            IConsole console,
            IBattleshipStateBuilder stateBuilder,
            IShowGameState gameShowService,
            BattleshipGameState gameState
        ) {
            _console = console;
            _stateBuilder = stateBuilder;
            _gameShowService = gameShowService;
            _gameState = gameState;
        }

        public void Play()
        {
            Show();

            while(!IsFinished())
            {
                string guess = _console.ReadLine();
                Play(guess);
            }
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
            catch (InvalidInputException)
            {
                _gameShowService.DisplayInputWarning();
            }
            catch (CellRepetitionException)
            {
                _gameShowService.DisplayRetryWarning();
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
                    cell => cell == BattleshipGridCell.Ship));
        }
    }
}