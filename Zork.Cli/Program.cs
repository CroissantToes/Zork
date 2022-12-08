using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Zork.Common;

namespace Zork.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            const string defaultGameFilename = @"Content\Game.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultGameFilename);
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFilename));

            var output = new ConsoleOutputService();
            var input = new ConsoleInputService();
            game.Run(input, output);

            while (game.IsRunning)
            {
                game.Output.Write("> ");
                input.ProcessInput();
            }

            output.WriteLine("Thank you for playing!");
        }

        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }
}