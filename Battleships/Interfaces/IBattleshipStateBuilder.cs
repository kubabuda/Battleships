using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface IBattleshipStateBuilder
    {
        BattleshipGameState InitialState();
        BattleshipGameState NextState(BattleshipGameState prevState, string guess);
    }
}