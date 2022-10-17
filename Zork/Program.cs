using System;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    public class Program
    {  
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            const string defaultGameFileName = @"Content/Zork.json";
            string gameFileName = (args.Length > 0 ? args[(int)CommandLineArguments.GameFileName] : defaultGameFileName);

            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFileName));
            game.Run(args);
        }

        private enum CommandLineArguments
        {
            GameFileName = 0
        }
    }
}