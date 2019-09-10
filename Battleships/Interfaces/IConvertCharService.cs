namespace Battleships.Interfaces
{
    public interface IConvertCharService
    {
        char GetLetter(int i);
        int GetLine(string guess);
        int GetColumn(string guess);
    }
}