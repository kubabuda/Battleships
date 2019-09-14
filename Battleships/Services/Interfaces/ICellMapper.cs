using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface ICellMapper
    {
        char GetCellValueToDisplay(BattleshipGridCell cell);
        BattleshipGridCell NewCellState(BattleshipGridCell prevCellState);
    }
}