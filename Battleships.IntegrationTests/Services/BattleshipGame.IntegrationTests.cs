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


        [Test]
        public void Play_ShouldShowGridWithSingleMiss_OnSingleShotMissed()
        {
            // arrange
            var expected =
            "  1 2 3 4 5 6 7 8 9 10\r\n" +
            "A     x               |\r\n" +
            "B                     |\r\n" +
            "C                     |\r\n" +
            "D                     |\r\n" +
            "E                     |\r\n" +
            "F                     |\r\n" +
            "G                     |\r\n" +
            "H                     |\r\n" +
            "I                     |\r\n" +
            "J                     |\r\n" +
            "  - - - - - - - - - - \r\n";

            // act
            _game.PlayRound("A3");

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }

        [Test]
        public void Play_ShouldShowHit_OnHit()
        {
            // arrange
            var prevState = _stateBuilder.Build();
            prevState.Grid[1][4] = BattleshipGridCell.Ship;
            _game = GameFromPrevState(_container, prevState);
            var expected =
            "  1 2 3 4 5 6 7 8 9 10\r\n" +
            "A                     |\r\n" +
            "B         *           |\r\n" +
            "C                     |\r\n" +
            "D                     |\r\n" +
            "E                     |\r\n" +
            "F                     |\r\n" +
            "G                     |\r\n" +
            "H                     |\r\n" +
            "I                     |\r\n" +
            "J                     |\r\n" +
            "  - - - - - - - - - - \r\n";

            // act
            _game.PlayRound("B5");

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }

        [TestCase("A11")]
        [TestCase("K1")]
        [TestCase("K11")]
        public void Play_ShouldShowWarning_OnInputOutOfRange(string guess)
        {
            // arrange
            var prevState = _stateBuilder.Build();
            prevState.Grid[1][4] = BattleshipGridCell.Ship;
            var expected = "Invalid cell, A-J and 1-10 are allowed\r\n";

            // act
            _game.PlayRound(guess);

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }

        [TestCase(BattleshipGridCell.Miss)]
        [TestCase(BattleshipGridCell.Hit)]
        public void Play_ShouldShowWarning_OnTouchingSameFieldAgain(BattleshipGridCell cellState)
        {
            // arrange
            var prevState = _stateBuilder.Build();
            prevState.Grid[1][4] = cellState;
            _game = GameFromPrevState(_container, prevState);
            var expected = "You already have shoot there, try something else\r\n";

            // act
            _game.PlayRound("B5");

            // assert
            Assert.AreEqual(expected, _consoleOut);
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