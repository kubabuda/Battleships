using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.IntegrationTests
{
    public class BattleshipGameTests
    {
        IConvertCharService charSvc;
        IConfiguration config;
        IConsole console;
        IBattleshipStateBuilder stateBuilder;

        private IBattleshipGame _game;
        private string consoleOut;

        [SetUp]
        public void SetUp()
        {
            config = new Configuration();
            consoleOut = "";
            console = Substitute.For<IConsole>();
            console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo => { 
                    var line = callinfo.ArgAt<string>(0);
                    consoleOut = $"{consoleOut}{line}\r\n";
                });
            charSvc = new ConvertCharService();
            stateBuilder = new BattleshipStateBuilder(charSvc, config);

            _game = new BattleshipGame(charSvc, config, console, stateBuilder);
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
            Assert.AreEqual(expectedFirstScreen, consoleOut);
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
            Assert.AreEqual(expected, consoleOut);
        }

        [Test]
        public void Play_ShouldShowHit_OnHit()
        {
            // arrange
            var prevState = stateBuilder.Build();
            prevState.Grid[1][4] = BattleshipGridCell.ShipUntouched;
            _game = new BattleshipGame(charSvc, config, console, stateBuilder, prevState);
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
            Assert.AreEqual(expected, consoleOut);
        }

        [TestCase("A11")]
        [TestCase("K1")]
        [TestCase("K11")]
        public void Play_ShouldShowWarning_OnInputOutOfRange(string guess)
        {
            // arrange
            _game = new BattleshipGame(charSvc, config, console, stateBuilder);
            var prevState = stateBuilder.Build();
            prevState.Grid[1][4] = BattleshipGridCell.ShipUntouched;
            var expected = "Invalid cell, A-J and 1-10 are allowed\r\n";

            // act
            _game.Play(guess);

            // assert
            Assert.AreEqual(expected, consoleOut);
        }

        [TestCase(BattleshipGridCell.Miss)]
        [TestCase(BattleshipGridCell.ShipHit)]
        public void Play_ShouldShowWarning_OnTouchingSameFieldAgain(BattleshipGridCell cellState)
        {
            // arrange
            var prevState = stateBuilder.Build();
            prevState.Grid[1][4] = cellState;
            _game = new BattleshipGame(charSvc, config, console, stateBuilder, prevState);
            var expected = "You already had shoot there, try something else\r\n";

            // act
            _game.Play("B5");

            // assert
            Assert.AreEqual(expected, consoleOut);
        }

        [TestCase(BattleshipGridCell.Empty, true)]
        [TestCase(BattleshipGridCell.Miss, true)]
        [TestCase(BattleshipGridCell.ShipHit, true)]
        [TestCase(BattleshipGridCell.ShipUntouched, false)]
        public void IsFinished_ShouldReturnTrueIfShipsLeft_GivenGameState(BattleshipGridCell cellState, bool expected)
        {
            // arrange
            var prevState = stateBuilder.Build();
            prevState.Grid[1][4] = cellState;
            _game = new BattleshipGame(charSvc, config, console, stateBuilder, prevState);

            // act
            var result = _game.IsFinished();

            // assert
            Assert.AreEqual(expected, result);
        }
    }
}