using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Models;
using System;

namespace Battleships.Services
{
    public class ReadUserGuessService: IReadUserGuess
    {
        private IBattleshipsConfiguration _configuration;
        private IConvertCharService _charService;

        public ReadUserGuessService(IBattleshipsConfiguration configuration,
            IConvertCharService charService
        )
        {   
            _charService = charService;
            _configuration = configuration;
        }

        public GridCoordinate GetCordinates(string guess)
        {
            try
            {
                var line = _charService.GetLine(guess);
                var column = _charService.GetColumn(guess);

                ValidateCoordinate(line);
                ValidateCoordinate(column);

                return new GridCoordinate(line, column);
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