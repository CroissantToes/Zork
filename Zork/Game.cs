using System;
using Newtonsoft.Json;
using System.IO;

namespace Zork
{
    internal class Game
    {
        public World World { get; set; }

        public Player Player { get; set; }

        public void Run(string[] args)
        {
            const string defaultRoomsFileName = @"Content/Zork.json";

            string roomsFileName = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFileName] : defaultRoomsFileName);

            InitializeRoomDescriptions(roomsFileName);

            Room previousRoom = null;

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine(currentRoom);

                if (previousRoom != currentRoom && !currentRoom.HasBeenVisited)
                {
                    Console.WriteLine(currentRoom.Description);
                    previousRoom = currentRoom;
                    currentRoom.HasBeenVisited = true;
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
                        outputString = currentRoom.Description;
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

        private static void InitializeRoomDescriptions(string roomsFileName)
        {
            _rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFileName));
        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse<Commands>(commandString, true, out Commands command) ? command : Commands.Unknown;
        }

        private static Room currentRoom
        {
            get
            {
                return _rooms[_location.row, _location.column];
            }
        }

        private static Room[,] _rooms;

        private static (int row, int column) _location = (1, 1);

        private enum Fields
        {
            Name = 0,
            Description
        }

        private enum CommandLineArguments
        {
            RoomsFileName = 0
        }
    }
}
