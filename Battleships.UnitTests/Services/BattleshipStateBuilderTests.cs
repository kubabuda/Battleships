using System.Collections.Generic;
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

        private char missMark = 'x';

        [SetUp]
        public void SetUp()
        {
            _charSvc = Substitute.For<IConvertCharService>();
            var config = Substitute.For<IConfiguration>();
            config.GridSize.Returns(10);
            config.EmptyGridDie.Returns(' ');
            _servceUnderTest = new BattleshipStateBuilder(_charSvc, config);
        }

        [Test]
        public void InitialState_ShouldReturnEmptyBoard_WithoutParameters()
        {
            // arrange
            var expected = new BattleshipGameState
            {
                Grid = GetEmptyGrid()
            };
            // act 
            var result = _servceUnderTest.InitialState();

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }


        [Test]
        public void NextState_ShouldReturnMissMark_WhenShotMissed()
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
            grid[1][1] = missMark;
            var expected = new BattleshipGameState
            {
                Grid = grid
            };

            // act 
            var result = _servceUnderTest.NextState(prev, guess);

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }


        private List<List<char>> GetEmptyGrid()
        {
            return new List<List<char>> {
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' },
                new List<char> { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' }
            };
        }
    }
}