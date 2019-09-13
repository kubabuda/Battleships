using Battleships.Configurations;
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

        public bool IsNextVertical()
        {
            return _random.Next() % 2 == 0;
        }

        public GridCoordinate NextCell()
        {
            return new GridCoordinate(NextCellIndex(), NextCellIndex());
        }

        private int NextCellIndex()
        {
            return _random.Next(_configuration.GridSize);
        }
    }
}