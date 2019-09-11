using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.UnitTests
{
    public class BattleshipGameTests
    {
        private BattleshipGame _game;

        [SetUp]
        public void SetUp()
        {
            var charService = Substitute.For<IConvertCharService>();
            var console = Substitute.For<IConsole>();
            var config = Substitute.For<IConfiguration>();
            var stateBuilder = Substitute.For<IBattleshipStateBuilder>();
            

            _game = new BattleshipGame(charService, config, console, stateBuilder);
        }
    }
}