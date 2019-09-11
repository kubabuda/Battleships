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
        private IBattleshipStateBuilder _servceUnderTest;

        private int gridSize = 10;
        private char empty = ' ';
        private char miss = 'x';
        private char hit = '*';

        [SetUp]
        public void SetUp()
        {
            _charSvc = Substitute.For<IConvertCharService>();
            var config = Substitute.For<IConfiguration>();
            config.GridSize.Returns(gridSize);
            config.EmptyGridDie.Returns(empty);
            config.MissMarker.Returns(miss);
            _servceUnderTest = new BattleshipStateBuilder(_charSvc, config);
        }

        [Test]
        public void Build_ShouldReturnEmptyBoard_WithoutParameters()
        {
            // arrange
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
            grid[1][1] = BattleshipGameDie.Miss;
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
            var guess = "C2";
            _charSvc.GetLine(guess).Returns(1);
            _charSvc.GetColumn(guess).Returns(1);
            var grid = GetEmptyGrid();
            grid[2][1] = BattleshipGameDie.ShipHit;
            var expected = new BattleshipGameState
            {
                Grid = grid
            };

            // act 
            var result = _servceUnderTest.Build(prev, guess);

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }


        private List<List<BattleshipGameDie>> GetEmptyGrid()
        {
            return new List<List<BattleshipGameDie>> {
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList(),
                Enumerable.Range(0, gridSize).Select(_ => BattleshipGameDie.Empty).ToList()
            };
        }
    }
}