using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class ShowGameStateServiceTests
    {
        private const int gridSize = 10;
        private string _consoleOut;
        
        private IShowGameState _showGameService;

        [SetUp]
        public void SetUp()
        {
            var charService = Substitute.For<IConvertCharService>();
            charService.GetLetter(0).Returns('A');
            charService.GetLetter(1).Returns('B');
            charService.GetLetter(2).Returns('C');
            charService.GetLetter(3).Returns('D');
            charService.GetLetter(4).Returns('E');
            charService.GetLetter(5).Returns('F');
            charService.GetLetter(6).Returns('G');
            charService.GetLetter(7).Returns('H');
            charService.GetLetter(8).Returns('I');
            charService.GetLetter(9).Returns('J');
            _consoleOut = "";
            var console = Substitute.For<IConsole>();
            console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo => { 
                    var line = callinfo.ArgAt<string>(0);
                    _consoleOut = $"{_consoleOut}{line}\r\n";
                });
            var config = Substitute.For<IConfiguration>();
            config.GridSize.Returns(gridSize);
            config.Empty.Returns(' ');
            config.Hit.Returns('*');
            config.Miss.Returns('x');
            _showGameService = new ShowGameStateService(charService, console, config);
        }

        [Test]
        public void Show_ShouldShowEmptyGrid_GivenEmptyGrid()
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

        [Test]
        public void Show_ShouldShowHitMissMarkers_GivenUsedGrid()
        {
            // arrange
            var expectedFirstScreen =
            "  1 2 3 4 5 6 7 8 9 10\r\n" +
            "A *                 x |\r\n" +
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
            var grid = BattleshipStateBuilderTests.GetEmptyGrid(10);
            grid[0][0] = BattleshipGridCell.ShipHit;
            grid[0][9] = BattleshipGridCell.Miss;
            var state = new BattleshipGameState { Grid = grid };
            
            // act
            _showGameService.Show(state);

            // assert
            Assert.AreEqual(expectedFirstScreen, _consoleOut);
        }

        [Test]
        public void DisplayInputWarning_DisplaysExpectedWarning_WithoutParameters()
        {
            // arrange
            var expected = "Invalid cell, A-J and 1-10 are allowed\r\n";

            // act
            _showGameService.DisplayInputWarning();

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }
    }
}