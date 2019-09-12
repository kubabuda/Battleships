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
        private IBattleshipStateBuilder _servceUnderTest;

        private int gridSize = 10;

        [SetUp]
        public void SetUp()
        {
            _charSvc = Substitute.For<IConvertCharService>();
            _config = Substitute.For<IConfiguration>();
            _config.GridSize.Returns(gridSize);
            _servceUnderTest = new BattleshipStateBuilder(_charSvc, _config);
        }

        [Test]
        public void Build_ShouldReturnEmptyBoard_WithoutParameters()
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


        [Test]
        public void Build_ShouldReturnBoardWithSingleShip_WhenSingleShipInConfiguration()
        {
            // arrange
            var x1 = 2;
            var x2 = 1;
            _config.Ships.Returns(new List<int>{ 1 });
            var expected = GetEmptyGrid();
            expected[2][1] = BattleshipGridCell.ShipUntouched;

            // act 
            var result = _servceUnderTest.Build();

            // assert
            Assert.AreEqual(expected, result.Grid);
        }

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