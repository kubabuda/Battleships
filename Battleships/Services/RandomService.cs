using Battleships.Interfaces;
using Battleships.Models;
using System;

namespace Battleships.Services
{
    public class RandomService : IRandom
    {
        private Random _random;
        private IConfiguration _configuration;

        public RandomService(IConfiguration configuration)
        {
            _configuration = configuration;
            _random = new Random();
        }

        public (int x, int y) NextCell()
        {
            return (x: NextCellIndex(), y: NextCellIndex());
        }

        private int NextCellIndex()
        {
            return _random.Next(_configuration.GridSize);
        }
    }
}