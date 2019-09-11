using Battleships.Interfaces;
using Battleships.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleships.Services
{
    public class BattleshipStateBuilder: IBattleshipStateBuilder
    {
        private const int _gridSize = 10;
        private IConvertCharService _charSvc { get; }
        
        public BattleshipStateBuilder(IConvertCharService charSvc)
        {
            _charSvc = charSvc;
        }

        public BattleshipGameState InitialState()
        {
            return new BattleshipGameState
            {
                Grid = new List<List<char>>(Enumerable.Range(0, _gridSize)
                    .Select(_ => new List<char>(
                        Enumerable.Range(0, _gridSize).Select(__ => ' ').ToList())
                    ).ToList())
            };
        }

        public BattleshipGameState NextState(BattleshipGameState prevState, string guess)
        {
            throw new NotImplementedException();
        }
    }
}