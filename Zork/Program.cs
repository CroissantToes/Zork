using System;

namespace Zork
{
    internal class Program
    {
        private static string currentRoom
        {
            get
            {
                return _rooms[_location.row, _location.column];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            bool isRunning = true;

            while (isRunning)
            {
                Console.Write($"{currentRoom}\n> ");
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
                        outputString = "This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door.";
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
                case Commands.North when _location.row < _rooms.GetLength(0):
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

        private static readonly string[,] _rooms = 
        {
            { "Rocky Trail", "South of House", "Canyon View"},
            { "Forest", "West of House", "Behind House" },
            { "Dense Woods", "North of House", "Clearing" }
        };

        private static (int row, int column) _location = (1, 1);
    }
}
