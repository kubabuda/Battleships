using Autofac;
using Battleships;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace Battleship.Services.IntegrationTests
{
    public class BattleshipGameIntegrationTests
    {
        IBattleshipsConfiguration _configuration;
        private IContainer _container;

        IBattleshipStateBuilder _stateBuilder;

        private BattleshipGame _game;
        private string _consoleOut;

        [SetUp]
        public void SetUp()
        {
            // setup mocked components
            var configuration = BattleshipsConfiguration.Default;
            configuration.Ships = new List<int>();
            _configuration = configuration;
            IConsole console = SetupConsoleMock();
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder(_configuration);
            // register mocks
            builder.RegisterInstance<IConsole>(console);
            builder.RegisterInstance<IBattleshipsConfiguration>(_configuration);

            _container = builder.Build();
            // get tested class instance
            _stateBuilder = _container.Resolve<IBattleshipStateBuilder>();
            _game = _container.Resolve<IBattleshipGame>() as BattleshipGame;
        }

        [TestCase(BattleshipGridCell.Empty, true)]
        [TestCase(BattleshipGridCell.Miss, true)]
        [TestCase(BattleshipGridCell.Hit, true)]
        [TestCase(BattleshipGridCell.Ship, false)]
        public void IsFinished_ShouldReturnTrueIfShipsLeft_GivenGameState(BattleshipGridCell cellState, bool expected)
        {
            // arrange
            var prevState = _stateBuilder.Build();
            prevState.Grid[1][4] = cellState;
            _game = GameFromPrevState(_container, prevState);

            // act
            var result = _game.IsFinished();

            // assert
            Assert.AreEqual(expected, result);
        }

        public IConsole SetupConsoleMock()
        {
            _consoleOut = "";
            var console = Substitute.For<IConsole>();
            console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo =>
                {
                    var line = callinfo.ArgAt<string>(0);
                    _consoleOut = $"{_consoleOut}{line}\r\n";
                });
            return console;
        }

        public static BattleshipGame GameFromPrevState(IContainer container, BattleshipGameState prevState)
        {
            return new BattleshipGame(
                container.Resolve<IConsole>(),
                container.Resolve<IBattleshipStateBuilder>(),
                container.Resolve<IShowGameState>(),
                prevState);
        }
    }
}