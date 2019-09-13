namespace Battleships.Models
{
    public class BattleShip
    {
        public int Length { get; set; }
        public bool IsVertical { get; set; }

        public BattleShip(int length, bool isVertical)
        {
            Length = length;
            IsVertical = isVertical;
        }
    }
}