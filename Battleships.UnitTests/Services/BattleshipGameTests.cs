using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class BattleshipGameTests
    {
        private BattleshipGame _game;

        [SetUp]
        public void SetUp()
        {
            var charService = Substitute.For<IConvertCharService>();
            var console = Substitute.For<IConsole>();
            var config = Substitute.For<IConfiguration>();
            var stateBuilder = Substitute.For<IBattleshipStateBuilder>();
            

            _game = new BattleshipGame(charService, config, console, stateBuilder);
        }
    }

    public class ShowGameStateServiceTests
    {
        private string _consoleOut;
        
        private IShowGameState _showGameService;

        [SetUp]
        public void SetUp()
        {
            _consoleOut = "";
            var console = Substitute.For<IConsole>();
            console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo => { 
                    var line = callinfo.ArgAt<string>(0);
                    _consoleOut = $"{_consoleOut}{line}\r\n";
                });
            _showGameService = new ShowGameStateService();
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
            var emptyGrid = BattleshipStateBuilderTests.GetEmptyGrid(10);
            var empty = new BattleshipGameState { Grid = emptyGrid };
            
            // act
            _showGameService.Show(empty);

            // assert
            Assert.AreEqual(expectedFirstScreen, _consoleOut);
        }
    }
}