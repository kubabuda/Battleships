namespace Battleships.Interfaces
{
    public interface IRandom
    {
        (int x, int y) NextCell();
        bool IsNextVertical();
    }
}