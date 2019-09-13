using Autofac;
using Battleships;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Services.IntegrationTests
{
    public class BattleshipStateBuilderIntegrationTests
    {
        private Configuration _config;
        private IBattleshipStateBuilder _stateBuilder;

        [SetUp]
        public void SetUp()
        {
            // setup mocked components
            _config = new Configuration();
            _config.Ships = new List<int>();
            var console = Substitute.For<IConsole>();
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder();
            // register mocks
            builder.RegisterInstance<IConsole>(console);
            builder.RegisterInstance<IConfiguration>(_config);
            
            var container = builder.Build();
            // get tested class instance
            _stateBuilder = container.Resolve<IBattleshipStateBuilder>();
        }

        [TestCase(new [] { 2, 3, 4 })]
        [TestCase(new [] { 4, 3, 3 })]
        public void Build_ShouldPlaceAllTheShips_FromConfiguration(IEnumerable<int> ships)
        {
            // arrange
            _config.Ships = new List<int>(ships);
            int expectedShips = ships.Sum();
            int expectedEmptyCells = (_config.GridSize * _config.GridSize) - expectedShips;

            // act
            var result = _stateBuilder.Build();

            // assert
            int shipsOnBoard = result.Grid.Sum(line => line.Where(c => c == BattleshipGridCell.ShipUntouched).Count());
            Assert.AreEqual(expectedShips, shipsOnBoard);
            int emptyCells = result.Grid.Sum(line => line.Where(c => c == BattleshipGridCell.Empty).Count());
            Assert.AreEqual(expectedEmptyCells, emptyCells);
        }

        public class CellRepetitionException: Exception { }
        public class InvalidInputException: Exception { }

        [Test]
        public void Build_ShouldThrowCellRepetitionException_WhenUserSelecstUsedCell()
        {
            // arrange
            var state = _stateBuilder.Build();
            state.Grid[0][0] = BattleshipGridCell.Miss;

            // act
            // assert
            Assert.Throws<CellRepetitionException>(() => _stateBuilder.Build(state, "A1"));
        }

        [TestCase("A11")]
        [TestCase("AA1")]
        [TestCase("M1")]
        [TestCase("MAAAA1")]
        [TestCase("FooBar")]
        public void Build_ShouldThrowInvalidInputException_WhenGuessIsInvalid(string invalidGuess)
        {
            // arrange
            var state = _stateBuilder.Build();

            // act
            // assert
            Assert.Throws<InvalidInputException>(() => _stateBuilder.Build(state, invalidGuess));
        }
    }
}