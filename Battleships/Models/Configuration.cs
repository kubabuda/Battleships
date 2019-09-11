namespace Battleships.Models
{
    public class Configuration : IConfiguration
    {
        public int GridSize => 10;
        public char Empty => ' ';
        public char Miss => 'x';
        public char Hit => '*';
    }
}