using System.Collections.Generic;

namespace Battleships.Models
{
    public class Configuration : IConfiguration
    {
        public int GridSize => 10;
        public char Empty => ' ';
        public char Miss => 'x';
        public char Hit => '*';
        public IEnumerable<int> Ships { get; set; }

        public Configuration()
        {
            Ships = new [] { 4, 3, 3};
        }
    }

}