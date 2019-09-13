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
            _config.Ships.Returns(new List<int> { 1 });
            var coord = new GridCoordinate(x, y);
            _randomService.NextCell().Returns(coord);
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
            _config.Ships.Returns(new List<int> { shipLength });
            var coord = new GridCoordinate(x, y);
            _randomService.NextCell().Returns(coord);
            _randomService.IsNextVertical().Returns(isVertical);
            var expected = GetEmptyGrid();
            expected[x][y] = BattleshipGridCell.Ship;
            expected[isVertical ? x + 1 : x][isVertical ? y : y + 1] = BattleshipGridCell.Ship;

            // act 
            var result = _servceUnderTest.Build();

            // assert
            Assert.AreEqual(expected, result.Grid);
        }

        [Test]
        public void GetShipStart_ShouldPreventCollisionsWithOtherShips_WhenPlacingNextShip()
        {
            // arrange
            var grid = GetEmptyGrid();
            var firstShipStart = new GridCoordinate(1, 2);
            grid[firstShipStart.line][firstShipStart.column] = BattleshipGridCell.Ship;    // place ship on grid
            grid[firstShipStart.line + 1][firstShipStart.column] = BattleshipGridCell.Ship;
            var nextShip = new BattleShip(2, true);
            var expected = new GridCoordinate(2, 3);
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

        [Test]
        public void Build_ShouldPlaceAllShips_FromLongestToSmallestInConfiguration()
        {
            var grid = GetEmptyGrid();
            var firstShipStart = new GridCoordinate(8, 4);
            grid[firstShipStart.line][firstShipStart.column] = BattleshipGridCell.Ship;    // 1st ship: 4 mast horizontal
            grid[firstShipStart.line][firstShipStart.column + 1] = BattleshipGridCell.Ship;
            grid[firstShipStart.line][firstShipStart.column + 2] = BattleshipGridCell.Ship;
            grid[firstShipStart.line][firstShipStart.column + 3] = BattleshipGridCell.Ship;
            var secondShipStart = new GridCoordinate(2, 3);
            grid[secondShipStart.line][secondShipStart.column] = BattleshipGridCell.Ship;    // 2nd ship: 3 mast vertical
            grid[secondShipStart.line + 1][secondShipStart.column] = BattleshipGridCell.Ship;
            grid[secondShipStart.line + 2][secondShipStart.column] = BattleshipGridCell.Ship;
            var thirdShipStart = new GridCoordinate(1, 2);
            grid[thirdShipStart.line][thirdShipStart.column] = BattleshipGridCell.Ship;    // 1st ship: 2 mast vertical
            grid[thirdShipStart.line + 1][thirdShipStart.column] = BattleshipGridCell.Ship;
            var ships = new[] { 2, 3, 4 };
            _config.Ships.Returns(ships);
            var starts = new[] { firstShipStart, secondShipStart, thirdShipStart };
            _randomService.NextCell().Returns(firstShipStart, secondShipStart, thirdShipStart);
            _randomService.IsNextVertical().Returns(false, true, true);
            _detectCollisionService.IsNextShipColliding(
                Arg.Any<List<List<BattleshipGridCell>>>(),
                Arg.Any<BattleShip>(),
                Arg.Is<GridCoordinate>(i => !starts.Contains(i))
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
            var coord = new GridCoordinate(1,1);
            _guessSvc.GetCordinates(guess).Returns(coord);
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
            var coord = new GridCoordinate(2,1);
            _guessSvc.GetCordinates(guess).Returns(coord);
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
        // TODO use game state builder
        private List<List<BattleshipGridCell>> GetEmptyGrid()
        {
            return EmptyGridBuilder.GetEmptyGrid(gridSize);
        }
    }
}