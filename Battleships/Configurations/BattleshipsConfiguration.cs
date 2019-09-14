using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Battleships.Configurations
{
    public class BattleshipsConfiguration : IBattleshipsConfiguration
    {
        public int GridSize { get; set; }
        public char Empty { get; set; }
        public char Miss { get; set; }
        public char Hit  { get; set; }
        public IEnumerable<int> Ships { get; set; }

        private BattleshipsConfiguration(IEnumerable<int> ships)
        {

            Ships = ships;
        }

        public static BattleshipsConfiguration Default
        {
            get
            {
                return new BattleshipsConfiguration(new [] { 4, 3, 3 })
                {
                    GridSize = 10,
                    Empty = ' ',
                    Miss = 'x',
                    Hit = '*',
                };
            }
        }
    }

    public static class BattleshipsConfigurationBuilder
    {
        public static BattleshipsConfiguration FromAppSettings(string filename)
        {
            IConfiguration configFile = new ConfigurationBuilder()
                .AddJsonFile(filename, true, true)
                .Build();

            var gameConfiguration = BattleshipsConfiguration.Default;
            
            if (configFile["GridSize"]!= null) { gameConfiguration.GridSize = int.Parse(configFile["GridSize"]); }
            if (configFile["Empty"]!= null) { gameConfiguration.Empty = configFile["Empty"][0]; }
            if (configFile["Miss"]!= null) { gameConfiguration.Miss = configFile["Miss"][0]; }
            if (configFile["Hit"]!= null) { gameConfiguration.Hit = configFile["Hit"][0]; }
            if (configFile["Ships"]!= null) { gameConfiguration.Ships = configFile.GetSection("Ships").Get<int[]>(); }

            return gameConfiguration;
        }
    }
}