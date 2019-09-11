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
}