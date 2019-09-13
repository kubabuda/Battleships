using Autofac;
using Battleships;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using NSubstitute;
using NUnit.Framework;

namespace Battleship.Services.IntegrationTests
{
    public class ReadUserGuessServiceIntegrationTests
    {
        private const int grid_size = 10;
        private IConfiguration _config;

        private IReadUserGuess _serviceUnderTest;

        [SetUp]
        public void SetUp()
        {
            // setup mocked components
            _config = Substitute.For<IConfiguration>();
            _config.GridSize.Returns(grid_size);
            // use DI setup for rest
            var builder = Bootstrapper.GetContainerBuilder();
            // register mocks
            builder.RegisterInstance<IConfiguration>(_config);
            
            var container = builder.Build();
            // get tested class instance
            _serviceUnderTest = container.Resolve<IReadUserGuess>();
        }

        [TestCase("B2", 1, 1)]
        [TestCase("A1", 0, 0)]
        [TestCase("A10", 0, 9)]
        [TestCase("J1", 9, 0)]
        [TestCase("J10", 9, 9)]
        public void GetCoordinates_ShouldReturnCoordinates_GivenValidGuess(string guess, int line, int column)
        {
            // arrange

            // act
            var result = _serviceUnderTest.GetCordinates(guess);

            // assert
            Assert.AreEqual(line, result.line);
            Assert.AreEqual(column, result.column);
        }


        [TestCase("B22")]
        [TestCase("AA1")]
        [TestCase("ASAP10")]
        [TestCase("K1")]
        [TestCase("FOO")]
        [TestCase("A0")]
        [TestCase("J0")]
        [TestCase("A-1")]
        [TestCase("J-2")]
        public void GetCoordinates_ShouldThrowException_GivenInvalidGuess(string guess)
        {
            // arrange

            // act
            // assert
            Assert.Throws<InvalidInputException>(() => _serviceUnderTest.GetCordinates(guess));
        }
    }
}