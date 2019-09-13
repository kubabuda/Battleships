using Battleships.Models;
using Battleships.Services;
using Battleships.UnitTests.TestUtils;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class DetectColisionServiceTests
    {
        private DetectColisionService _servceUnderTest = new DetectColisionService();
        private const int gridSize = 10;

        [TestCase(1, 2, true, true)]
        [TestCase(0, 2, true, true)]
        [TestCase(4, 4, true, false)]
        [TestCase(1, 1, false, true)]
        public void IsGuessColliding_ReturnsTrue_ForShipsCollision(int x, int y, bool isVertical, bool expected)
        {
            // arrange
            var grid = EmptyGridBuilder.GetEmptyGrid(gridSize);
            grid[1][2] = BattleshipGridCell.Ship;    // place ship on grid
            grid[2][2] = BattleshipGridCell.Ship;
            var ship = new BattleShip() { Length = 2, IsVertical = isVertical };

            // act
            var result = _servceUnderTest.IsNextShipColliding(grid, ship, (x: x, y: y));

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(0, 0, 2, true, false)]
        [TestCase(0, 0, 2, false, false)]
        [TestCase(0, 9, 1, true, false)]
        [TestCase(0, 9, 1, false, false)]
        [TestCase(0, 9, 2, true, false)]
        [TestCase(0, 9, 2, false, true)]
        [TestCase(9, 0, 1, true, false)]
        [TestCase(9, 0, 1, false, false)]
        [TestCase(9, 0, 2, true, true)]
        [TestCase(9, 0, 2, false, false)]
        [TestCase(9, 9, 1, true, false)]
        [TestCase(9, 9, 1, false, false)]
        [TestCase(9, 9, 2, true, true)]
        [TestCase(9, 9, 2, false, true)]
        public void IsGuessColliding_ReturnsTrue_WhenShipWouldExtendOverBorder(int x, int y, int length, bool isVertical, bool expected)
        {
            var grid = EmptyGridBuilder.GetEmptyGrid(gridSize);
            var ship = new BattleShip() { Length = length, IsVertical = isVertical };

            // act
            var result = _servceUnderTest.IsNextShipColliding(grid, ship, (x: x, y: y));

            // assert
            Assert.AreEqual(expected, result);
        }
    }
}