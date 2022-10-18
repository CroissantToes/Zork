using System;
using Newtonsoft.Json;
using System.IO;

namespace Zork
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(world, startingLocation);
        }

        public void Run(string[] args)
        {
            Room previousRoom = null;

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine(Player.currentRoom);

                if (previousRoom != Player.currentRoom && !Player.currentRoom.HasBeenVisited)
                {
                    Console.WriteLine(Player.currentRoom.Description);
                    previousRoom = Player.currentRoom;
                    Player.currentRoom.HasBeenVisited = true;
                }

                Console.Write("> ");

                string inputString = Console.ReadLine().Trim();
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
                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
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
