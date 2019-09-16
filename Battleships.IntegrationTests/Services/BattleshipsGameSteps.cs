using Autofac;
using Battleship.Services.IntegrationTests;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Battleships.IntegrationTests.Services
{
    [Binding]
    public class BattleshipsGameSteps
    {
        private string _consoleOut;
        private BattleshipsConfiguration _configuration;
        private IConsole _console;
        private IContainer _container;
        private BattleshipGame _game;

        [Given(@"New Battleships game")]
        public void GivenNewBattleshipsGame()
        {
            // setup mocked components
            _configuration = BattleshipsConfiguration.Default;
            _console = Substitute.For<IConsole>();
            _consoleOut = "";
            _console
                .When(c => c.WriteLine(Arg.Any<string>()))
                .Do(callinfo =>
                {
                    var line = callinfo.ArgAt<string>(0);
                    _consoleOut = $"{_consoleOut}{line}\r\n";
                });
            ContainerBuilder builder = PrepareBuilder();
            _container = builder.Build();
            // get tested class instance
            _game = (BattleshipGame)_container.Resolve<IBattleshipGame>();
        }

        [Given(@"No ships on grid")]
        public void GivenNoShipsOnGrid()
        {
            // no ships in config
            _configuration.Ships = new List<int>();
            // re-read configuration
            _game = (BattleshipGame)_container.Resolve<IBattleshipGame>();
        }

        [Given(@"Ships in folowing grid points")]
        public void GivenShipsInFolowingGridPoints(Table table)
        {
            // prepare state for test case
            var state = BattleshipGameState.Empty(_configuration.GridSize);
            foreach (var row in table.Rows)
            {
                var line = int.Parse(row["line"]) - 1;
                var column = int.Parse(row["column"]) - 1;
                state.Grid[line][column] = BattleshipGridCell.Ship;
            }
            // get tested class instance
            _game = BattleshipGameIntegrationTests.GameFromPrevState(_container, state);
        }

        [When(@"Game play starts")]
        public void WhenGamePlaysSingleRound()
        {
            _game.Play();
        }

        [Given(@"I type grid coordinates")]
        public void GivenITypeGridCoordinates(Table table)
        {
            var userInputs = table.Rows.Select(tr => tr["value"]).ToArray();

            _console.ReadLine().Returns(userInputs[0], userInputs.Skip(1).ToArray());
        }

        [Then(@"Game is finished")]
        public void ThenGameIsFinished()
        {
            Assert.IsTrue(_game.IsFinished());
        }

        [Then(@"Empty grid was displayed")]
        public void ThenEmptyGridIsDisplayed()
        {
            var expected =
            "  1 2 3 4 5 6 7 8 9 10\r\n" +
            "A                     |\r\n" +
            "B                     |\r\n" +
            "C                     |\r\n" +
            "D                     |\r\n" +
            "E                     |\r\n" +
            "F                     |\r\n" +
            "G                     |\r\n" +
            "H                     |\r\n" +
            "I                     |\r\n" +
            "J                     |\r\n" +
            "  - - - - - - - - - - \r\n";

            Assert.AreEqual(expected, _consoleOut);
        }

        private ContainerBuilder PrepareBuilder()
        {
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder(_configuration);
            // register mocks
            builder.RegisterInstance<IConsole>(_console);
            builder.RegisterInstance<IBattleshipsConfiguration>(_configuration);
            return builder;
        }
    }
}
