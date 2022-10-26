using System;
using Newtonsoft.Json;
using System.IO;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public IOutputService Output { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(world, startingLocation);
        }

        public void Run(string[] args, IOutputService output)
        {
            Output = output;

            Room previousRoom = null;

            bool isRunning = true;

            while (isRunning)
            {
                Output.WriteLine(Player.currentRoom);

                if (previousRoom != Player.currentRoom && !Player.currentRoom.HasBeenVisited)
                {
                    Output.WriteLine(Player.currentRoom.Description);
                    previousRoom = Player.currentRoom;
                    Player.currentRoom.HasBeenVisited = true;
                }

                Output.Write("> ");

                string inputString = Output.ReadLine().Trim();
                Commands command = ToCommand(inputString);

                string outputString;
                switch (command)
                {
                    case Commands.Quit:
                        outputString = "Thank you for playing!";
                        isRunning = false;
                        break;
                    case Commands.Look:
                        outputString = Player.currentRoom.Description;
                        break;
                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction) == true)
                        {
                            outputString = $"You moved {command}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;
                    case Commands.Score:
                        outputString = $"Your score would be {Player.Score}, in {Player.Moves} move(s).";
                        break;
                    case Commands.Reward:
                        Player.Score++;
                        outputString = $"Your score has increased! Your new score is {Player.Score}.";
                        break;
                    default:
                        outputString = "Unknown command.";
                        Player.Moves--;
                        break;
                }

                Player.Moves++;

                Output.WriteLine(outputString);
            }
        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse<Commands>(commandString, true, out Commands command) ? command : Commands.Unknown;
        }

        private enum Fields
        {
            Name = 0,
            Description
        }
    }   
}
