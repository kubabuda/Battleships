using System.Collections.Generic;
using System.Linq;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using Battleships.UnitTests.TestUtils;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class BattleshipStateBuilderTests
    {
        private IReadUserGuess _guessSvc;
        private IConfiguration _config;
        private IDetectColisionService _detectCollisionService;
        private IRandom _randomService;

        private BattleshipStateBuilder _servceUnderTest;

        private int gridSize = 10;

        [SetUp]
        public void SetUp()
        {
            _guessSvc = Substitute.For<IReadUserGuess>();
            _config = Substitute.For<IConfiguration>();
            _randomService = Substitute.For<IRandom>();
            _config.GridSize.Returns(gridSize);
            _detectCollisionService = Substitute.For<IDetectColisionService>();

            _servceUnderTest = new BattleshipStateBuilder(
                _guessSvc,
                 _config,
                 _detectCollisionService,
                 _randomService);
        }

        // ------------------- Initial state --------------------------- //

        [Test]
        public void Build_ShouldReturnEmptyBoard_WhenNoShipsConfigured()
        {
            // arrange
            _config.Ships.Returns(new List<int>());
            var expected = new BattleshipGameState
            {
                Grid = GetEmptyGrid()
            };
            // act 
            var result = _servceUnderTest.Build();

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }


        [TestCase(1, 2)]
        [TestCase(4, 9)]
        [TestCase(0, 0)]
        [TestCase(9, 9)]
        public void Build_ShouldReturnBoardWithSingleShip_WhenSingleShipInConfiguration(int x, int y)
        {
            // arrange
            _config.Ships.Returns(new List<int>{ 1 });
            _randomService.NextCell().Returns((x:x, y:y));
            var expected = GetEmptyGrid();
            expected[x][y] = BattleshipGridCell.Ship;

            // act 
            var result = _servceUnderTest.Build();

            // assert
            Assert.AreEqual(expected, result.Grid);
        }

        [TestCase(true, 2)]
        [TestCase(false, 2)]
        public void Build_ShouldRotateShip_WhenItsSelectedRandomly(bool isVertical, int shipLength)
        {
            // arrange
            int x = 1;
            int y = 1;
            _config.Ships.Returns(new List<int>{ shipLength });
            _randomService.NextCell().Returns((x:x, y:y));
            _randomService.IsNextVertical().Returns(isVertical);
            var expected = GetEmptyGrid();
            expected[x][y] = BattleshipGridCell.Ship;
            expected[isVertical? x + 1 : x][isVertical? y : y + 1] = BattleshipGridCell.Ship;

            // act 
            var result = _servceUnderTest.Build();

            // assert
            Assert.AreEqual(expected, result.Grid);
        }

        [Test]
        public void GetShipStart_ShouldPreventCollisionsWithOtherShips_WhenPlacingNextShip() {
            // arrange
            var grid = GetEmptyGrid();
            var firstShipStart = (x: 1, y: 2);
            grid[firstShipStart.x][firstShipStart.y] = BattleshipGridCell.Ship;    // place ship on grid
            grid[firstShipStart.x + 1][firstShipStart.y] = BattleshipGridCell.Ship;
            var nextShip = new BattleShip() { Length = 2, IsVertical = true };
            var expected = (x: 2, y: 3);
            _randomService.NextCell()
                .Returns(firstShipStart,    // we randomly got space that is 
                    firstShipStart,         // already used by other ships
                    expected);          // until we got proper one
            _detectCollisionService.IsNextShipColliding(grid, nextShip, firstShipStart).Returns(true);
            _detectCollisionService.IsNextShipColliding(grid, nextShip, expected).Returns(false);
            
            // act
            var result = _servceUnderTest.GetShipStart(grid, nextShip);

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test] // TODO test in integration too
        public void Build_ShouldPlaceAllShips_FromLongestToSmallestInConfiguration()
        {
            var grid = GetEmptyGrid();
            var firstShipStart = (x: 8, y: 4);
            grid[firstShipStart.x][firstShipStart.y] = BattleshipGridCell.Ship;    // 1st ship: 4 mast horizontal
            grid[firstShipStart.x][firstShipStart.y + 1] = BattleshipGridCell.Ship;
            grid[firstShipStart.x][firstShipStart.y + 2] = BattleshipGridCell.Ship;
            grid[firstShipStart.x][firstShipStart.y + 3] = BattleshipGridCell.Ship;
            var secondShipStart = (x: 2, y: 3);
            grid[secondShipStart.x][secondShipStart.y] = BattleshipGridCell.Ship;    // 2nd ship: 3 mast vertical
            grid[secondShipStart.x + 1][secondShipStart.y] = BattleshipGridCell.Ship;
            grid[secondShipStart.x + 2][secondShipStart.y] = BattleshipGridCell.Ship;
            var thirdShipStart = (x: 1, y: 2);
            grid[thirdShipStart.x][thirdShipStart.y] = BattleshipGridCell.Ship;    // 1st ship: 2 mast vertical
            grid[thirdShipStart.x + 1][thirdShipStart.y] = BattleshipGridCell.Ship;
            var ships = new [] { 2, 3, 4 };
            _config.Ships.Returns(ships);
            var starts = new [] { firstShipStart, secondShipStart, thirdShipStart };
            _randomService.NextCell().Returns(firstShipStart, secondShipStart, thirdShipStart);
            _randomService.IsNextVertical().Returns(false, true, true);
            _detectCollisionService.IsNextShipColliding(
                Arg.Any<List<List<BattleshipGridCell>>>(),
                Arg.Any<BattleShip>(),
                Arg.Is<(int, int)>(i => !starts.Contains(i))
            ).Returns(true);
            // act
            var result = _servceUnderTest.Build();

            // assert
            Assert.AreEqual(grid, result.Grid);
        }

        // ------------------- Next state ------------------------- //

        [Test]
        public void Build_ShouldReturnMissMark_WhenShotMissed()
        {
            // arrange
            var prev = new BattleshipGameState
            {
                Grid = GetEmptyGrid()
            };
            var guess = "B2";
            _guessSvc.GetCordinates(guess).Returns((1, 1));
            var grid = GetEmptyGrid();
            grid[1][1] = BattleshipGridCell.Miss;
            var expected = new BattleshipGameState
            {
                Grid = grid
            };

            // act 
            var result = _servceUnderTest.Build(prev, guess);

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }

        [Test]
        public void Build_ShouldReturnHitMark_WhenShotHit()
        {
            // arrange
            var prev = new BattleshipGameState
            {
                Grid = GetEmptyGrid()
            };
            prev.Grid[2][1] = BattleshipGridCell.Ship;
            var guess = "C2";
            _guessSvc.GetCordinates(guess).Returns((2, 1));
            var expected = new BattleshipGameState
            {
                Grid = GetEmptyGrid()
            };
            expected.Grid[2][1] = BattleshipGridCell.Hit;

            // act 
            var result = _servceUnderTest.Build(prev, guess);

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }

        //  ------------------- Helper methods -------------------- //
        private List<List<BattleshipGridCell>> GetEmptyGrid()
        {
            return EmptyGridBuilder.GetEmptyGrid(gridSize);
        }
    }
}