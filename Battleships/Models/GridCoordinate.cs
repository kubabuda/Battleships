namespace Battleships.Models
{
    public class GridCoordinate
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public GridCoordinate(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }
}