using System.Collections.Generic;
using System.Linq;
using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class BattleshipStateBuilderTests
    {
        private IConvertCharService _charSvc;
        private IConfiguration _config;
        private IRandom _randomService;

        private BattleshipStateBuilder _servceUnderTest;

        private int gridSize = 10;

        [SetUp]
        public void SetUp()
        {
            _charSvc = Substitute.For<IConvertCharService>();
            _config = Substitute.For<IConfiguration>();
            _randomService = Substitute.For<IRandom>();
            _config.GridSize.Returns(gridSize);
            _servceUnderTest = new BattleshipStateBuilder(_charSvc, _config, _randomService);
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
            expected[x][y] = BattleshipGridCell.ShipUntouched;

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
            expected[x][y] = BattleshipGridCell.ShipUntouched;
            expected[isVertical? x + 1 : x][isVertical? y : y + 1] = BattleshipGridCell.ShipUntouched;

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
            grid[firstShipStart.x][firstShipStart.y] = BattleshipGridCell.ShipUntouched;    // place ship on grid
            grid[firstShipStart.x + 1][firstShipStart.y] = BattleshipGridCell.ShipUntouched;
            var nextShip = new BattleShip() { length = 2, isVertical = true };
            var expected = (x: 2, y: 3);
            _randomService.NextCell()
                .Returns(firstShipStart,    // we randomly got space that is 
                    firstShipStart,         // already used by other ships
                    expected);          // until we got proper one
            
            // act
            var result = _servceUnderTest.GetShipStart(grid, nextShip);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(1, 2, true)]
        [TestCase(0, 2, true)]
        [TestCase(4, 4, false)]
        public void IsGuessCollidingWithShips_ReturnsTrue_ForShipsCollision(int x, int y, bool expected)
        {
            // arrange
            var grid = GetEmptyGrid();
            grid[1][2] = BattleshipGridCell.ShipUntouched;    // place ship on grid
            grid[2][2] = BattleshipGridCell.ShipUntouched;
            var ship = new BattleShip() { length = 2, isVertical = true };

            // act
            var result = _servceUnderTest.IsGuessCollidingWithShips(grid, ship, (x: x, y: y));

            // assert
            Assert.AreEqual(expected, result);
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
            _charSvc.GetLine(guess).Returns(1);
            _charSvc.GetColumn(guess).Returns(1);
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
            prev.Grid[2][1] = BattleshipGridCell.ShipUntouched;
            var guess = "C2";
            _charSvc.GetLine(guess).Returns(2);
            _charSvc.GetColumn(guess).Returns(1);
            var expected = new BattleshipGameState
            {
                Grid = GetEmptyGrid()
            };
            expected.Grid[2][1] = BattleshipGridCell.ShipHit;

            // act 
            var result = _servceUnderTest.Build(prev, guess);

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }

        //  ------------------- Helper methods -------------------- //

        private List<List<BattleshipGridCell>> GetEmptyGrid()
        {
            return new List<List<BattleshipGridCell>>
            {
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGridCell.Empty).ToList()
            };
        }
    }
}