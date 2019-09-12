using Autofac;
using Battleships;
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
        IConfiguration _config;
        private IContainer _container;

        IBattleshipStateBuilder _stateBuilder;

        private IBattleshipGame _game;
        private string _consoleOut;

        [SetUp]
        public void SetUp()
        {
            // setup mocked components
            var config = new Configuration();
            config.Ships = new List<int>();
            _config = config;
            _consoleOut = "";
            var console = Substitute.For<IConsole>();
            console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo => { 
                    var line = callinfo.ArgAt<string>(0);
                    _consoleOut = $"{_consoleOut}{line}\r\n";
                });
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder();
            // register mocks
            builder.RegisterInstance<IConsole>(console);
            builder.RegisterInstance<IConfiguration>(_config);
            
            _container = builder.Build();
            // get tested class instance
            _stateBuilder = _container.Resolve<IBattleshipStateBuilder>();
            _game = _container.Resolve<IBattleshipGame>();
        }

        [Test]
        public void Show_ShouldShowUntouchedGrid_OnFirstRound()
        {
            // arrange
            var expectedFirstScreen =
            "  1 2 3 4 5 6 7 8 9 10\r\n" +
            "A                     |\r\n" +
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
            _game.Show();

            // assert
            Assert.AreEqual(expectedFirstScreen, _consoleOut);
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
            _game.Play("A3");

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }

        [Test]
        public void Play_ShouldShowHit_OnHit()
        {
            // arrange
            var prevState = _stateBuilder.Build();
            prevState.Grid[1][4] = BattleshipGridCell.ShipUntouched;
            _game = GameFromPrevState(prevState);
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
            _game.Play("B5");

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
            prevState.Grid[1][4] = BattleshipGridCell.ShipUntouched;
            var expected = "Invalid cell, A-J and 1-10 are allowed\r\n";

            // act
            _game.Play(guess);

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }

        [TestCase(BattleshipGridCell.Miss)]
        [TestCase(BattleshipGridCell.ShipHit)]
        public void Play_ShouldShowWarning_OnTouchingSameFieldAgain(BattleshipGridCell cellState)
        {
            // arrange
            var prevState = _stateBuilder.Build();
            prevState.Grid[1][4] = cellState;
            _game = GameFromPrevState(prevState);
            var expected = "You already had shoot there, try something else\r\n";

            // act
            _game.Play("B5");

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }

        [TestCase(BattleshipGridCell.Empty, true)]
        [TestCase(BattleshipGridCell.Miss, true)]
        [TestCase(BattleshipGridCell.ShipHit, true)]
        [TestCase(BattleshipGridCell.ShipUntouched, false)]
        public void IsFinished_ShouldReturnTrueIfShipsLeft_GivenGameState(BattleshipGridCell cellState, bool expected)
        {
            // arrange
            var prevState = _stateBuilder.Build();
            prevState.Grid[1][4] = cellState;
            _game = GameFromPrevState(prevState);

            // act
            var result = _game.IsFinished();

            // assert
            Assert.AreEqual(expected, result);
        } 
    
        private IBattleshipGame GameFromPrevState(BattleshipGameState prevState)
        {
            return new BattleshipGame(
                _container.Resolve<IConvertCharService>(),
                _container.Resolve<IConfiguration>(),
                _container.Resolve<IConsole>(),
                _container.Resolve<IBattleshipStateBuilder>(),
                _container.Resolve<IShowGameState>(),
                prevState);
        }
    }
}