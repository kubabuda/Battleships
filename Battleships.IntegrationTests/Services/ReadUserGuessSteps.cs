using Autofac;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using NSubstitute;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Battleships.IntegrationTests.Services
{
    [Binding]
    public class ReadUserGuessSteps
    {
        private IReadUserGuess _serviceUnderTest;
        private string _guess;

        [Given(@"New reader instance")]
        public void GivenNewReaderInstance()
        {
            // setup mocked components
            var _config = Substitute.For<IBattleshipsConfiguration>();
            var grid_size = 10;
            _config.GridSize.Returns(grid_size);
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder(_config);
            // register mocks
            builder.RegisterInstance<IBattleshipsConfiguration>(_config);

            var container = builder.Build();
            // get tested class instance
            _serviceUnderTest = container.Resolve<IReadUserGuess>();
        }

        [When(@"I read '(.*)'")]
        public void WhenIRead(string guess)
        {
            _guess = guess;
        }

        [Then(@"'(.*)' '(.*)' are read")]
        public void ThenAreRead(int line, int column)
        {
            var result = _serviceUnderTest.GetCordinates(_guess);

            Assert.AreEqual(line, result.Line);
            Assert.AreEqual(column, result.Column);
        }

        [Then(@"InvalidInputException is thrown by reader")]
        public void ThenInvalidInputExceptionIsThrownByReader()
        {
            Assert.Throws<InvalidInputException>(() => _serviceUnderTest.GetCordinates(_guess));
        }
    }
}
