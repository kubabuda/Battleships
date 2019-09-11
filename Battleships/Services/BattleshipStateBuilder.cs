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

        public BattleshipGameState Build()
        {
            return new BattleshipGameState
            {
                Grid = new List<List<BattleshipGameDie>>(Enumerable.Range(0, _configuration.GridSize)
                    .Select(_ => new List<BattleshipGameDie>(
                        Enumerable.Range(0,  _configuration.GridSize).Select(__ => BattleshipGameDie.Empty).ToList())
                    ).ToList())
            };
        }

        public BattleshipGameState Build(BattleshipGameState prevState, string guess)
        {
            var lineNo = _charSvc.GetLine(guess);
            var colNo = _charSvc.GetColumn(guess);

            var newState = new BattleshipGameState 
            {
                Grid = prevState.Grid   // shallow copy
            };
            newState.Grid[lineNo][colNo] = 
                newState.Grid[lineNo][colNo] == BattleshipGameDie.ShipUntouched ?
                    BattleshipGameDie.ShipHit :
                    BattleshipGameDie.Miss;

            return newState;
        }
    }
}