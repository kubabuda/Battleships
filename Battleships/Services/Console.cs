
namespace Battleships.Services
{
    public class Console: IConsole {
        public Console()
        {
            
        }

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