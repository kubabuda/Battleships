using Battleships.Models;

namespace Battleships.Interfaces
{
    public interface IShowGameState
    {
        void Show(BattleshipGameState state);
        void DisplayInputWarning();
    }
}