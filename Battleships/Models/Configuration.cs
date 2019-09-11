namespace Battleships.Models
{
    public class Configuration : IConfiguration
    {
        public int GridSize => 10;
        public char EmptyGridDie => ' ';

        public char MissMarker => 'x';
    }

}