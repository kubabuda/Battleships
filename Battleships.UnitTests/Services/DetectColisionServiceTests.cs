using Battleships.Models;
using Battleships.Services;
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
            var grid = BattleshipStateBuilderTests.GetEmptyGrid(gridSize);
            grid[1][2] = BattleshipGridCell.ShipUntouched;    // place ship on grid
            grid[2][2] = BattleshipGridCell.ShipUntouched;
            var ship = new BattleShip() { length = 2, isVertical = isVertical };

            // act
            var result = _servceUnderTest.IsGuessColliding(grid, ship, (x: x, y: y));

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(1, 2, true, true)]
        [TestCase(0, 2, true, true)]
        [TestCase(4, 4, true, false)]
        [TestCase(1, 1, false, true)]
        public void IsGuessCollidingWithShips_ReturnsTrue_ForShipsCollision(int x, int y, bool isVertical, bool expected)
        {
            // arrange
            var grid = BattleshipStateBuilderTests.GetEmptyGrid(gridSize);
            grid[1][2] = BattleshipGridCell.ShipUntouched;    // place ship on grid
            grid[2][2] = BattleshipGridCell.ShipUntouched;
            var ship = new BattleShip() { length = 2, isVertical = isVertical };

            // act
            var result = _servceUnderTest.IsGuessCollidingWithShips(grid, ship, (x: x, y: y));

            // assert
            Assert.AreEqual(expected, result);
        }
    }
}