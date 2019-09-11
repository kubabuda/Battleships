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

        [SetUp]
        public void SetUp()
        {
            _charSvc = Substitute.For<IConvertCharService>();
            _servceUnderTest = new BattleshipStateBuilder(_charSvc);
        }

        [Test]
        public void InitialState_ShouldReturnEmptyBoard_WithoutParameters(){
            // arrange
            var expectedGrid = new List<List<char>> {
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
            var expected = new BattleshipGameState 
            {
                Grid = expectedGrid
            };
            
            // act 
            var result = _servceUnderTest.InitialState();

            // assert
            Assert.AreEqual(expected.Grid, result.Grid);
        }
    }
}