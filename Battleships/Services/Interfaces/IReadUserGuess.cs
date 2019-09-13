using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface IReadUserGuess
    {
        GridCoordinate GetCordinates(string guess);
    }
}