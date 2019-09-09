﻿using Autofac;
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

            builder.RegisterType<Battleships.Services.Console>().As<IConsole>();
            
            return builder.Build();
        }
    }
}