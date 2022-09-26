using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    internal class Game
    {
        public World World { get; set; }

        public Player Player { get; set; }

        public void Run()
        {
            string roomsFileName = @"Content\Rooms.txt";

            InitializeRoomDescriptions(roomsFileName);

            Room previousRoom = null;

            bool isRunning = true;

            while (isRunning)
            {
                Console.Write($"{Player.currentRoom}\n> ");

                if (previousRoom != Player.currentRoom && !Player.currentRoom.HasBeenVisited)
                {
                    Console.WriteLine(Player.currentRoom.Description);
                    previousRoom = Player.currentRoom;
                    Player.currentRoom.HasBeenVisited = true;
                }

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
                        if (Player.Move(command))
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

        private void InitializeRoomDescriptions(string roomsFileName)
        {
            _rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFileName));
        }

        private static Room[,] _rooms;

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
