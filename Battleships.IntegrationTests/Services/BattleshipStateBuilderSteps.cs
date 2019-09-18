using Autofac;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Battleships.IntegrationTests.Services
{
    [Binding]
    public class BattleshipStateBuilderSteps
    {
        private BattleshipsConfiguration _config;
        private IBattleshipStateBuilder _stateBuilder;
        private BattleshipGameState _state;
        private Func<BattleshipGameState> _getNextState;

        [Given(@"New state builder instance")]
        public void GivenNewStateBuilderInstance()
        {
            // setup mocked components
            _config = BattleshipsConfiguration.Default;
            _config.Ships = new List<int>();
            var console = Substitute.For<IConsole>();
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder(_config);
            // register mocks
            builder.RegisterInstance(console);
            builder.RegisterInstance<IBattleshipsConfiguration>(_config);

            var container = builder.Build();
            // get tested class instance
            _stateBuilder = container.Resolve<IBattleshipStateBuilder>();
        }

        [Given(@"I have entered '(.*)' into configuration")]
        public void GivenIHaveEnteredIntoConfiguration(string ships)
        {
            _config.Ships = GetShipLengths(ships);
        }

        [Given(@"'(.*)' in prev state '(.*)' '(.*)'")]
        public void GivenInPrevState(string state, int line, int column)
        {
            _state.Grid[line - 1][column - 1] = BattleshipsGameSteps.GetState(state);
        }

        [Given(@"I have previous game state")]
        public void GivenIHavePreviousGameState()
        {
            _state = _stateBuilder.Build();
        }

        [When(@"I generate new game state")]
        public void WhenIGenerateNewGameState()
        {
            _state = _stateBuilder.Build();
        }

        [When(@"I generate next state with '(.*)'")]
        public void WhenIGenerateNextStateWith(string guess)
        {
            _getNextState = () => _stateBuilder.Build(_state, guess);
        }

        [Then(@"All '(.*)' are placed on board")]
        public void ThenAllArePlacedOnBoard(string ships)
        {
            var configuredShipCount = GetShipLengths(ships).Sum();

            var shipsPlaced = _state.Grid.Sum(gl => gl.Where(c => c == BattleshipGridCell.Ship).Count());

            Assert.AreEqual(configuredShipCount, shipsPlaced);
        }

        [Then(@"'(.*)' '(.*)' is in '(.*)'")]
        public void ThenIsIn(int line, int column, string state)
        {
            var nextState = _getNextState();
            Assert.AreEqual(BattleshipsGameSteps.GetState(state), nextState.Grid[line - 1][column - 1]);
        }

        [Then(@"InvalidInputException is thrown")]
        public void ThenInvalidInputExceptionIsThrown()
        {
            Assert.Throws<InvalidInputException>(() => _getNextState());
        }

        [Then(@"CellRepetitionException is thrown")]
        public void ThenCellRepetitionExceptionIsThrown()
        {
            Assert.Throws<CellRepetitionException>(() => _getNextState());
        }

        private static int[] GetShipLengths(string ships)
        {
            return ships.Split(",").Select(s => int.Parse(s)).ToArray();
        }
    }
}
