using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System;

namespace Battleships.Services
{
    public class ReadUserGuessService: IReadUserGuess
    {
        private IConfiguration _configuration;
        private IConvertCharService _charSvc; // todo use either service or svc

        public ReadUserGuessService(IConfiguration config,
            IConvertCharService charSvc
        )
        {   
            _charSvc = charSvc;
            _configuration = config; // todo rename either config or configuration
        }

        public (int line, int column) GetCordinates(string guess) // todo get coordinates
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
            if (line < 0
                | line >= _configuration.GridSize)
            {
                throw new InvalidInputException();
            }
        }
    }
}