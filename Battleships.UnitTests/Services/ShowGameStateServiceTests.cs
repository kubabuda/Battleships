using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using Battleships.UnitTests.TestUtils;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class ShowGameStateServiceTests
    {
        private const int gridSize = 10;
        private string _consoleOut;
        
        private IShowGameState _serviceUnderTests;

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
            _serviceUnderTests = new ShowGameStateService(charService, console, config);
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
            var emptyGrid = EmptyGridBuilder.GetEmptyGrid(10);
            var empty = new BattleshipGameState { Grid = emptyGrid };
            
            // act
            _serviceUnderTests.Show(empty);

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
            var grid = EmptyGridBuilder.GetEmptyGrid(10);
            grid[0][0] = BattleshipGridCell.ShipHit;
            grid[0][9] = BattleshipGridCell.Miss;
            var state = new BattleshipGameState { Grid = grid };
            
            // act
            _serviceUnderTests.Show(state);

            // assert
            Assert.AreEqual(expectedFirstScreen, _consoleOut);
        }

        [Test]
        public void DisplayInputWarning_DisplaysExpectedWarning_WithoutParameters()
        {
            // arrange
            var expected = "Invalid cell, A-J and 1-10 are allowed\r\n";

            // act
            _serviceUnderTests.DisplayInputWarning();

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }

        [Test]
        public void DisplayRetryWarning_DisplaysExpectedWarning_WithoutParameters()
        {
            // arrange
            var expected = "You already had shoot there, try something else\r\n";

            // act
            _serviceUnderTests.DisplayRetryWarning();

            // assert
            Assert.AreEqual(expected, _consoleOut);
        }
    }

    public class BattleshipGameTests {
        private IConsole _console;
        private IBattleshipStateBuilder _stateBuilder;
        private IShowGameState _gameShowService;

        private BattleshipGame _serviceUnderTests;

        private BattleshipGameState initialState;
        int gridSize = 4;
        private BattleshipGameState state1;
        private BattleshipGameState state2;
        private BattleshipGameState state3;

        [SetUp]
        public void SetUp()
        {
            initialState = new BattleshipGameState() 
            { 
                Grid = EmptyGridBuilder.GetEmptyGrid(gridSize) 
            };
            initialState.Grid[0][0] = BattleshipGridCell.ShipUntouched;
            initialState.Grid[0][1] = BattleshipGridCell.ShipUntouched;
            initialState.Grid[0][2] = BattleshipGridCell.ShipUntouched;

            state1 = new BattleshipGameState() 
            { 
                Grid = EmptyGridBuilder.GetEmptyGrid(gridSize) 
            };
            state1.Grid[0][0] = BattleshipGridCell.ShipHit;
            state1.Grid[0][1] = BattleshipGridCell.ShipUntouched;
            state1.Grid[0][2] = BattleshipGridCell.ShipUntouched;

            state2 = new BattleshipGameState() 
            { 
                Grid = EmptyGridBuilder.GetEmptyGrid(gridSize) 
            };
            state2.Grid[0][0] = BattleshipGridCell.ShipHit;
            state2.Grid[0][1] = BattleshipGridCell.ShipHit;
            state2.Grid[0][2] = BattleshipGridCell.ShipUntouched;

            state3 = new BattleshipGameState() 
            { 
                Grid = EmptyGridBuilder.GetEmptyGrid(gridSize) 
            };
            state3.Grid[0][0] = BattleshipGridCell.ShipHit;
            state3.Grid[0][1] = BattleshipGridCell.ShipHit;
            state3.Grid[0][2] = BattleshipGridCell.ShipHit;

            _console = Substitute.For<IConsole>();
            _stateBuilder = Substitute.For<IBattleshipStateBuilder>();
            _stateBuilder.Build().Returns(initialState);
            _gameShowService = Substitute.For<IShowGameState>();

            _serviceUnderTests = new BattleshipGame(_console,
                _stateBuilder,
                _gameShowService);
        }

        [Test]
        public void Play_ShowsGame_GivenGuess()
        {
            // arrange
            var guess = "A1";
            var nextState = new BattleshipGameState();
            _stateBuilder.Build(initialState, guess).Returns(nextState);

            // act
            _serviceUnderTests.Play(guess);

            // assert
            _gameShowService.Received().Show(nextState);
        }

        [Test]
        public void Play_PlaysUntilAllShipsSank_Parameterless()
        {
            // arrange
            var guess1 = "A1";
            _stateBuilder.Build(initialState, guess1).Returns(state1);

            var guess2 = "A2";
            _stateBuilder.Build(state1, guess2).Returns(state2);

            var guess3 = "A3";
            _stateBuilder.Build(state2, guess3).Returns(state3);
            _console.ReadLine().Returns(guess1, guess2, guess3);

            // act
            _serviceUnderTests.Play();

            // assert
            Assert.IsTrue(_serviceUnderTests.IsFinished());
        }

        [Test]
        public void Play_ShowsAllGeneratedStates_Parameterless()
        {
            // arrange
            var guess1 = "A1";
            _stateBuilder.Build(initialState, guess1).Returns(state1);

            var guess2 = "A2";
            _stateBuilder.Build(state1, guess2).Returns(state2);

            var guess3 = "A3";
            _stateBuilder.Build(state2, guess3).Returns(state3);
            _console.ReadLine().Returns(guess1, guess2, guess3);

            // act
            _serviceUnderTests.Play();

            // assert
            _gameShowService.Received().Show(initialState);
            _gameShowService.Received().Show(state1);
            _gameShowService.Received().Show(state2);
            _gameShowService.Received().Show(state3);

        }
    }
}