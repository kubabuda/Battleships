using Autofac;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Battleships.IntegrationTests.Services
{
    [Binding]
    public class BattleshipsGameSteps
    {
        private BattleshipsConfiguration _configuration;
        private IConsole _console;
        private IContainer _container;
        private BattleshipGame _game;

        [Given(@"New Battleships game")]
        public void GivenNewBattleshipsGame()
        {
            // setup mocked components
            _configuration = BattleshipsConfiguration.Default;
            _console = Substitute.For<IConsole>();
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder(_configuration);
            // register mocks
            builder.RegisterInstance<IConsole>(_console);
            builder.RegisterInstance<IBattleshipsConfiguration>(_configuration);

            _container = builder.Build();
            // get tested class instance
            _game = (BattleshipGame)_container.Resolve<IBattleshipGame>();
        }

        [Given(@"Ships in folowing grid points")]
        public void GivenShipsInFolowingGridPoints()
        {
            _configuration.Ships = new List<int>();
            _game = (BattleshipGame)_container.Resolve<IBattleshipGame>();
        }

        [When(@"Game plays single round")]
        public void WhenGamePlaysSingleRound()
        {
            _game.Play();
        }

        [Then(@"Game is finished")]
        public void ThenGameIsFinished()
        {
            Assert.IsTrue(_game.IsFinished());
        }
    }
}
