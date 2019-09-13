namespace Battleships.Models
{
    public class GridCoordinate
    {
        // TODO naming convention
        public int line { get; set; }
        public int column { get; set; }

        public GridCoordinate(int l, int col)
        {
            line = l;
            column = col;
        }
    }
}