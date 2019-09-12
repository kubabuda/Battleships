using System.Collections.Generic;
using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.IntegrationTests
{
    public class BattleshipGameTests
    {
        IConvertCharService _charSvc;
        IConfiguration _config;
        IConsole _console;
        IRandom _random;
        IBattleshipStateBuilder _stateBuilder;

        private IBattleshipGame _game;
        private string _consoleOut;

        [SetUp]
        public void SetUp()
        {
            // TODO make DI part of integration tests
            var config = new Configuration();
            config.Ships = new List<int>();
            _config = config;
            _consoleOut = "";
            _console = Substitute.For<IConsole>();
            _console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo => { 
                    var line = callinfo.ArgAt<string>(0);
                    _consoleOut = $"{_consoleOut}{line}\r\n";
                });
            _charSvc = new ConvertCharService();
            _random = new RandomService(_config);
            _stateBuilder = new BattleshipStateBuilder(_charSvc, _config, _random);

            _game = new BattleshipGame(_charSvc, _config, _console, _stateBuilder);
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
            _game = new BattleshipGame(_charSvc, _config, _console, _stateBuilder, prevState);
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
            _game = new BattleshipGame(_charSvc, _config, _console, _stateBuilder);
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
            _game = new BattleshipGame(_charSvc, _config, _console, _stateBuilder, prevState);
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
            _game = new BattleshipGame(_charSvc, _config, _console, _stateBuilder, prevState);

            // act
            var result = _game.IsFinished();

            // assert
            Assert.AreEqual(expected, result);
        }
    }
}