using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using Battleships.UnitTests.TestUtils;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
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
            initialState.Grid[0][0] = BattleshipGridCell.Ship;
            initialState.Grid[0][1] = BattleshipGridCell.Ship;
            initialState.Grid[0][2] = BattleshipGridCell.Ship;

            state1 = new BattleshipGameState() 
            { 
                Grid = EmptyGridBuilder.GetEmptyGrid(gridSize) 
            };
            state1.Grid[0][0] = BattleshipGridCell.Hit;
            state1.Grid[0][1] = BattleshipGridCell.Ship;
            state1.Grid[0][2] = BattleshipGridCell.Ship;

            state2 = new BattleshipGameState() 
            { 
                Grid = EmptyGridBuilder.GetEmptyGrid(gridSize) 
            };
            state2.Grid[0][0] = BattleshipGridCell.Hit;
            state2.Grid[0][1] = BattleshipGridCell.Hit;
            state2.Grid[0][2] = BattleshipGridCell.Ship;

            state3 = new BattleshipGameState() 
            { 
                Grid = EmptyGridBuilder.GetEmptyGrid(gridSize) 
            };
            state3.Grid[0][0] = BattleshipGridCell.Hit;
            state3.Grid[0][1] = BattleshipGridCell.Hit;
            state3.Grid[0][2] = BattleshipGridCell.Hit;

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