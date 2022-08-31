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
            Console.WriteLine(command);
        }

        static Commands ToCommand(string commandString)
        {
            return Enum.TryParse<Commands>(commandString, true, out Commands command) ? command : Commands.Unknown;
        }

        static bool IsEven(int value)
        {
            return value % 2 == 0 ? true : false;
        }
    }
}
