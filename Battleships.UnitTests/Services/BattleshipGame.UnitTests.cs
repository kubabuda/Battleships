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
        private string consoleOut;

        [SetUp]
        public void SetUp()
        {
            _console = Substitute.For<IConsole>();
            _game = new BattleshipGame(_console);
        }

        [TestCase(0, 'A')]
        [TestCase(1, 'B')]
        public void GetLetter_ShouldReturnNthLetter_GivenN(int n, char expected)
        {
            // arrange

            // act
            var result = _game.GetLetter(n);

            // assert
            Assert.AreEqual(expected, result);
        }
    }
}