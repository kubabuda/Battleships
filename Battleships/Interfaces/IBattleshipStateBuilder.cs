using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface IBattleshipStateBuilder
    {
        BattleshipGameState Build();
        BattleshipGameState Build(BattleshipGameState prevState, string guess);
    }
}