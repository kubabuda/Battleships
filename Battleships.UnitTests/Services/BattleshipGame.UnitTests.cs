using Battleships.Interfaces;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class BattleshipGameTests
    {
        private IConsole _console;
        private BattleshipGame _game;
        private IConvertCharService _charSvc;
        private string consoleOut;

        [SetUp]
        public void SetUp()
        {
            _console = Substitute.For<IConsole>();
            _charSvc = Substitute.For<IConvertCharService>();
            _game = new BattleshipGame(_console, _charSvc);
        }
    }

    public class ConvertCharServiceTests
    {
        private IConsole _console;
        private ConvertCharService _serviceUnderTests;
        private string consoleOut;

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


        [TestCase("A1", 0)]
        [TestCase("C3", 2)]
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