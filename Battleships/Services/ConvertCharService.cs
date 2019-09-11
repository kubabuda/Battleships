using Battleships.Interfaces;
using System;

namespace Battleships.Services
{
    public class ConvertCharService: IConvertCharService {
        private const int _ASCI_A = 65;
        
        public char GetLetter(int i)
        {
            return Convert.ToChar(_ASCI_A + i);
        }

        public int GetLine(string guess) {
            return Convert.ToInt32(guess[0]) - _ASCI_A;
        }

        public int GetColumn(string guess) {
            return int.Parse(guess.Substring(1)) - 1;
        }
    }
}