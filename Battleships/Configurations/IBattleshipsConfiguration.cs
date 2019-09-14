using System.Collections.Generic;

namespace Battleships.Configurations
{
    public interface IBattleshipsConfiguration
    {
        int GridSize { get; }
        char Empty { get; }
        char Miss { get; }
        char Hit { get; }

        IEnumerable<int> Ships { get; }
    }
}