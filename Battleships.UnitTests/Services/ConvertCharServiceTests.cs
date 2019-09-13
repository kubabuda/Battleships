using Battleships.Services;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class ConvertCharServiceTests
    {
        private ConvertCharService _serviceUnderTests;

        [SetUp]
        public void SetUp()
        {
            _serviceUnderTests = new ConvertCharService();
        }

        [TestCase(0, 'A')]
        [TestCase(1, 'B')]
        public void GetLetter_ShouldReturnNthLetter_GivenN(int n, char expected)
        {
            // arrange

            // act
            var result = _serviceUnderTests.GetLetter(n);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("A1", 0)]
        [TestCase("C3", 2)]
        public void GetLine_ShouldReturnExpectedCoulumn_GivenGuess(string guess, int expected)
        {
            // arrange

            // act
            var result = _serviceUnderTests.GetLine(guess);

            // assert
            Assert.AreEqual(expected, result);
        }

        [TestCase("a1", 0)]
        [TestCase("c3", 2)]
        public void GetLine_ShouldReturnExpectedCoulumn_GivenLowercaseGuess(string guess, int expected)
        {
            // arrange

            // act
            var result = _serviceUnderTests.GetLine(guess);

            // assert
            Assert.AreEqual(expected, result);
        }


        [TestCase("A1", 0)]
        [TestCase("C3", 2)]
        [TestCase("B5", 4)]
        [TestCase("B10", 9)]
        public void GetColumn_ShouldReturnExpectedCoulumn_GivenGuess(string guess, int expected)
        {
            // arrange

            // act
            var result = _serviceUnderTests.GetColumn(guess);

            // assert

            Assert.AreEqual(expected, result);
        }
    }
}