using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            string inputString = Console.ReadLine().Trim();

            Console.WriteLine(inputString);
            Commands command = ToCommand(inputString);

            if(command == Commands.Quit)
            {
                Console.WriteLine("Thank you for playing.");
            }
            else if (inputString == "LOOK")
            {
                Console.WriteLine("This is an open field west of a white house with a boarded front door.\n A rubber mat saying 'Welcome to Zork!' lies by the front door.");
            }
            else
            {
                Console.WriteLine($"Unknown Command: {inputString}");
            }
        }

        static Commands ToCommand(string commandString)
        {
            if(Enum.TryParse<Commands>(commandString, true, out Commands command))
            {
                return command;
            }
            else
            {
                return Commands.Unknown;
            }
        }

        static bool IsEven(int value)
        {
            return value % 2 == 0 ? true : false;
        }
    }
}
