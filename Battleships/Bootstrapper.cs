using Autofac;
using Battleships.Interfaces;
using Battleships.Models;
using Battleships.Services;

namespace Battleships
{
    public static class Bootstrapper
    {
        public static IContainer GetContainer()
        {
            ContainerBuilder builder = GetContainerBuilder();

            return builder.Build();
        }

        public static ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();

            // configurations
            builder.RegisterType<Configuration>().As<IConfiguration>();
            // services
            builder.RegisterType<BattleshipGame>().As<IBattleshipGame>();
            builder.RegisterType<BattleshipStateBuilder>().As<IBattleshipStateBuilder>();
            builder.RegisterType<ConvertCharService>().As<IConvertCharService>();
            builder.RegisterType<RandomService>().As<IRandom>();
            // I/O wrappers
            builder.RegisterType<Battleships.Services.Console>().As<IConsole>();
            return builder;
        }
    }
}