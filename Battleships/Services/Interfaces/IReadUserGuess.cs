namespace Battleships.Interfaces
{
    public interface IReadUserGuess
    {
        (int line, int column) GetCordinates(string guess);
    }
}