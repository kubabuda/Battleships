using Autofac;
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
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder(_configuration);
            // register mocks
            builder.RegisterInstance<IConsole>(_console);
            builder.RegisterInstance<IBattleshipsConfiguration>(_configuration);

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
            _game = GameFromPrevState(_container, state);
        }

        [Given(@"'(.*)' in (.*), (.*)")]
        public void GivenCellStateIn(string cellState, int line, int column)
        {
            // prepare state for test case
            var state = BattleshipGameState.Empty(_configuration.GridSize);

            state.Grid[line][column] = GetState(cellState);

            // get tested class instance
            _game = GameFromPrevState(_container, state);
        }
        
        [When(@"Game play starts")]
        public void WhenGamePlaysSingleRound()
        {
            _game.Play();
        }

        [Given(@"I play round with '(.*)' guess")]
        public void GivenIPlayRoundWith(string guess)
        {
            _game.PlayRound(guess);
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

        [Then(@"Game is not finished")]
        public void ThenGameIsNotFinished()
        {
            Assert.IsFalse(_game.IsFinished());
        }

        [Then(@"Empty grid was displayed(.*) times")]
        public void ThenEmptyGridIsDisplayed(int expected)
        {
            var empty =
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

            Assert.AreEqual(expected, CountInConsoleOut(empty));
        }

        [Then(@"Grid was displayed(.*) times")]
        public void ThenGridWasDisplayed(int times)
        {
            var gridHeader = "  1 2 3 4 5 6 7 8 9 10\r\n" +
            "A";
            var gridHeadersInOutput = CountInConsoleOut(gridHeader);

            Assert.AreEqual(times, gridHeadersInOutput);
        }

        [Then(@"Console was displaying")]
        public void ThenConsoleWasDisplaying(string multilineText)
        {
            Assert.True(_consoleOut.Contains(multilineText.TrimEnd()));
        }

        [Then(@"Game displayed shot twice warning (.*) times")]
        public void ThenShotTwiceWarningWasDisplayed(int expected)
        {
            var warning = "You already have shoot there, try something else\r\n";
            var timesInOutput = CountInConsoleOut(warning);

            Assert.AreEqual(expected, timesInOutput);
        }

        [Then(@"Game displayed invalid input warning (.*) times")]
        public void ThenInvalidInputWasDisplayed(int expected)
        {
            var warning = "Invalid cell, A-J and 1-10 are allowed\r\n";
            var timesInOutput = CountInConsoleOut(warning);

            Assert.AreEqual(expected, timesInOutput);
        }

        [Then(@"Miss mark was displayed (.*) times")]
        public void ThenMissMarkWasDisplayed(int expected)
        {;
            var timesInOutput = CountInConsoleOut($"{_configuration.Miss}");

            Assert.AreEqual(expected, timesInOutput);
        }

        [Then(@"Hit mark was displayed (.*) times")]
        public void ThenHitMarkWasDisplayed(int expected)
        {
            ;
            var timesInOutput = CountInConsoleOut($"{_configuration.Hit}");

            Assert.AreEqual(expected, timesInOutput);
        }

        public static BattleshipGame GameFromPrevState(IContainer container, BattleshipGameState prevState)
        {
            return new BattleshipGame(
                container.Resolve<IConsole>(),
                container.Resolve<IBattleshipStateBuilder>(),
                container.Resolve<IShowGameState>(),
                prevState);
        }

        private static Dictionary<string, BattleshipGridCell> _stateMappings = new Dictionary<string, BattleshipGridCell>
        {
            { "HIT", BattleshipGridCell.Hit },
            { "MISS", BattleshipGridCell.Miss },
            { "SHIP", BattleshipGridCell.Ship },
            { "EMPTY", BattleshipGridCell.Empty }
        };

        private BattleshipGridCell GetState(string state)
        {
            return _stateMappings[state.ToUpper()];
        }


        private int CountInConsoleOut(string gridHeader)
        {
            return _consoleOut.Split(gridHeader).Count() - 1;
        }
    }
}
