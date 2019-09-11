using System.Collections.Generic;
using Battleships.Interfaces;
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
            // _servceUnderTest = new BattleshipGame(_console, _charSvc);
        }

        [Test]
        public void InitialState_ShouldReturnEmptyBoard_WithoutParameters(){
            // arrange
            var expected = new List<List<char>> {
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
            
            // act 
            var result = _servceUnderTest.InitialState();

            // assert
            Assert.AreEqual(expected, result);
        }
    }
}