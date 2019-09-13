using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface IRandom
    {
        GridCoordinate NextCell();
        bool IsNextVertical();
    }
}