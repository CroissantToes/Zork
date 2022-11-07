﻿using System;
using System.IO;
using Newtonsoft.Json;
using Zork.Common;

namespace Zork.Cli
{
    internal class Program
    {  
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            const string defaultGameFileName = @"Content/Zork.json";
            string gameFileName = (args.Length > 0 ? args[(int)CommandLineArguments.GameFileName] : defaultGameFileName);

            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFileName));

            var input = new ConsoleInputService();
            var output = new ConsoleOutputService();

            game.Run(args, output, input);

            while (game.IsRunning)
            {
                input.ProcessInput();
            }
        }

        private enum CommandLineArguments
        {
            GameFileName = 0
        }
    }
}