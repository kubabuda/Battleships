using Autofac;
using Battleships.Configurations;
using Battleships.Interfaces;
using Battleships.Services;

namespace Battleships
{
    public static class Bootstrapper
    {
        public static IContainer GetContainer(IBattleshipsConfiguration configuration)
        {
            ContainerBuilder builder = GetContainerBuilder(configuration);

            return builder.Build();
        }

        public static ContainerBuilder GetContainerBuilder(IBattleshipsConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            // configurations
            builder.RegisterInstance(configuration).As<IBattleshipsConfiguration>();
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
