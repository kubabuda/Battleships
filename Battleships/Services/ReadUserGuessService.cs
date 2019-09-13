using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System;

namespace Battleships.Services
{
    public class ReadUserGuessService: IReadUserGuess
    {
        private IConfiguration _configuration;
        private IConvertCharService _charSvc;

        public ReadUserGuessService(IConfiguration config,
            IConvertCharService charSvc
        )
        {   
            _charSvc = charSvc;
            _configuration = config;
        }

        public (int line, int column) GetCordinates(string guess) 
        {
            try
            {
                var line = _charSvc.GetLine(guess);
                var column = _charSvc.GetColumn(guess);

                ValidateCoordinate(line);
                ValidateCoordinate(column);

                return (line: line, column: column);
            }
            catch (Exception)
            {
                throw new InvalidInputException();
            }
        }

        private void ValidateCoordinate(int line)
        {
            if (line >= _configuration.GridSize)
            {
                throw new InvalidInputException();
            }
        }
    }
}