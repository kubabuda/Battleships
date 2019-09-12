﻿using System;
using Autofac;
using Battleships.Interfaces;

namespace Battleships
{
    class Program
    {
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            System.Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            Container = Bootstrapper.GetContainer();

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
                while(Console.ReadLine().ToLower() == "y");

                Console.WriteLine("OK. Bye!");
            }
            System.Console.CancelKeyPress -= new ConsoleCancelEventHandler(Console_CancelKeyPress);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            System.Console.WriteLine("Understandable. Have a nice day!");
        }
    }
}
