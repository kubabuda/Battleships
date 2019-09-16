using System;
using Autofac;
using Battleships.Configurations;
using Battleships.Interfaces;

namespace Battleships
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            System.Console.CancelKeyPress += Console_CancelKeyPress;

            var configuration = BattleshipsConfigurationBuilder.FromAppSettings("appsettings.json");

            Container = Bootstrapper.GetContainer(configuration);

            using (var scope = Container.BeginLifetimeScope())
            {
                Console.WriteLine("Welcome to Battleships game!");

                do
                {
                    var game = scope.Resolve<IBattleshipGame>();
                    game.Play();

                    Console.WriteLine("You won. Congratulations!");
                    Console.WriteLine("Play again? (Y/N)");
                }
                while (string.Equals(Console.ReadLine(), "y", StringComparison.OrdinalIgnoreCase));

                Console.WriteLine("OK. Bye!");
            }
            System.Console.CancelKeyPress -= Console_CancelKeyPress;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            System.Console.WriteLine("Understandable. Have a nice day!");
        }
    }
}
