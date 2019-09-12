﻿using Autofac;
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
                var console = scope.Resolve<IConsole>();
                console.WriteLine("Welcome to Battleships game!");
                
                var game = scope.Resolve<IBattleshipGame>();
                game.Play();

                console.WriteLine("You won. Congratulations!");
            }
        }
    }
}
