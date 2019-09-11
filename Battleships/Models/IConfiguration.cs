namespace Battleships.Models
{
    public interface IConfiguration
    {
        int GridSize { get; }
        char EmptyGridDie { get; }
    }

}