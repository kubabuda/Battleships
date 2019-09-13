using Battleships.Interfaces;

namespace Battleships.Services
{
    public class Console : IConsole
    {
        public string ReadLine()
        {
            return System.Console.ReadLine();
        }

        public void WriteLine(string line)
        {
            System.Console.WriteLine(line);
        }
    }
}