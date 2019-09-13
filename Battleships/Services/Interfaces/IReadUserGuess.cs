namespace Battleships.Interfaces
{
    public interface IReadUserGuess
    {
        // todo extract type GridCoordinate
        (int line, int column) GetCordinates(string guess);
    }
}