namespace Battleships.Interfaces
{
    public interface IBattleshipGame
    {
        void Show();
        void Play(string guess);
        bool IsFinished();
    }
}