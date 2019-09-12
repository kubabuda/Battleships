using Autofac;
using Battleships.Interfaces;

namespace Battleships
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            Container = Bootstrapper.GetContainer();

            using (var scope = Container.BeginLifetimeScope())
            {
                var game = scope.Resolve<IBattleshipGame>();
                var console = scope.Resolve<IConsole>();
                console.WriteLine("Hello");
            }
        }
    }
}
