using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Zork
{
    internal class Program
    {
        private static Room currentRoom
        {
            get
            {
                return _rooms[_location.row, _location.column];
            }
        }

        static void Main(string[] args)
        {
            const string defaultRoomsFileName = @"Content/Rooms.json";

            string roomsFileName = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFileName] : defaultRoomsFileName);

            InitializeRoomDescriptions(roomsFileName);

            Console.WriteLine("Welcome to Zork!");

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
                        if (Move(command))
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

        private static bool Move(Commands command)
        {
            bool didMove;

            switch (command)
            {
                case Commands.North when _location.row < _rooms.GetLength(0) - 1:
                    _location.row++;
                    didMove = true;
                    break;
                case Commands.South when _location.row > 0:
                    _location.row--;
                    didMove = true;
                    break;
                case Commands.East when _location.column < _rooms.GetLength(1) - 1:
                    _location.column++;
                    didMove = true;
                    break;
                case Commands.West when _location.column > 0:
                    _location.column--;
                    didMove = true;
                    break;
                default:
                    didMove = false;
                    break;
            }
            return didMove;
        }

        private static void InitializeRoomDescriptions(string roomsFileName)
        {
            _rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFileName));
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
