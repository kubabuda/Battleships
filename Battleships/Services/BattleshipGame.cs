using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipGame: IBattleshipGame
    {
        private readonly IBattleshipStateBuilder _stateBuilder;
        private readonly IShowGameState _gameShowService;

        private BattleshipGameState _gameState;

        public BattleshipGame(
            IBattleshipStateBuilder stateBuilder,
            IShowGameState gameShowService)
        :this(stateBuilder, gameShowService, stateBuilder.Build()) { }

        public BattleshipGame(
            IBattleshipStateBuilder stateBuilder,
            IShowGameState gameShowService,
            BattleshipGameState gameState
        ) {
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
                _gameShowService.DisplayInputWarning();
            }
            catch (InvalidOperationException)
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
                    cell => cell == BattleshipGridCell.ShipUntouched));
        }

        public void Play()
        {
            throw new NotImplementedException();
        }
    }
}