namespace Battleships.Models
{
    public interface IConfiguration
    {
        int GridSize { get; }
        char Empty { get; }
        char Miss { get; }
        char Hit { get; }
    }

}