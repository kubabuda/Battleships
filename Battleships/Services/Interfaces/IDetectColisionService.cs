using System.Collections.Generic;
using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface IDetectColisionService
    {
        bool IsNextShipColliding(List<List<BattleshipGridCell>> grid,
            BattleShip nextShip,
            GridCoordinate nextShipStartCell);
    }
}