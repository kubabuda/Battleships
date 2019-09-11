using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipStateBuilder: IBattleshipStateBuilder
    {
        private IConfiguration _configuration;
        private IConvertCharService _charSvc { get; }
        
        public BattleshipStateBuilder(IConvertCharService charSvc, IConfiguration config)
        {
            _charSvc = charSvc;
            _configuration = config;
        }

        public BattleshipGameState InitialState()
        {
            return new BattleshipGameState
            {
                Grid = new List<List<char>>(Enumerable.Range(0, _configuration.GridSize)
                    .Select(_ => new List<char>(
                        Enumerable.Range(0,  _configuration.GridSize).Select(__ => _configuration.EmptyGridDie).ToList())
                    ).ToList())
            };
        }

        public BattleshipGameState NextState(BattleshipGameState prevState, string guess)
        {
            throw new NotImplementedException();
        }
    }
}