using Autofac;
using Battleships.Configurations;
using Battleships.Interfaces;
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
            // I/O wrappers
            builder.RegisterType<Battleships.Services.Console>().As<IConsole>();
            // services
            builder.RegisterType<BattleshipGame>().As<IBattleshipGame>();
            builder.RegisterType<BattleshipStateBuilder>().As<IBattleshipStateBuilder>();
            builder.RegisterType<ConvertCharService>().As<IConvertCharService>();
            builder.RegisterType<RandomService>().As<IRandom>();
            builder.RegisterType<DetectColisionService>().As<IDetectColisionService>();
            builder.RegisterType<ShowGameStateService>().As<IShowGameState>();
            builder.RegisterType<ReadUserGuessService>().As<IReadUserGuess>();
            builder.RegisterType<CellMapper>().As<ICellMapper>();

            return builder;
        }
    }
}
