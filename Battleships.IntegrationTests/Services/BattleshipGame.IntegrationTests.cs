using Battleships.Interfaces;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.IntegrationTests
{
    public class BattleshipGameTests
    {
        private IConsole _console;
        private IBattleshipGame _game;
        private string consoleOut;

        [SetUp]
        public void SetUp()
        {
            consoleOut = "";
            _console = Substitute.For<IConsole>();
            _console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo => { 
                    var line = callinfo.ArgAt<string>(0);
                    consoleOut = $"{consoleOut}{line}\r\n";
                });
            var charSvc = new ConvertCharService();

            _game = new BattleshipGame(_console, charSvc);
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
    }
}