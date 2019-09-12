using Battleships.Configurations;
using Battleships.Interfaces;
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

        public bool IsNextVertical()
        {
            return _random.Next() % 2 == 0;
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