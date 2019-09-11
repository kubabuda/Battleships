using Autofac;
using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;
using System;

namespace Battleships
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            Container = GetContainer();

            using (var scope = Container.BeginLifetimeScope())
            {
                var console = scope.Resolve<IConsole>();
                console.WriteLine("Hello");
            }
        }

        private static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            // configurations
            builder.RegisterType<Configuration>().As<IConfiguration>();
            // services
            builder.RegisterType<BattleshipGame>().As<IBattleshipGame>();
            builder.RegisterType<BattleshipStateBuilder>().As<IBattleshipStateBuilder>();
            builder.RegisterType<ConvertCharService>().As<IConvertCharService>();
            // I/O wrappers
            builder.RegisterType<Battleships.Services.Console>().As<IConsole>();
            
            return builder.Build();
        }
    }
}
