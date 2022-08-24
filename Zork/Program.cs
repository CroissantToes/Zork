using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            string inputString = Console.ReadLine().Trim().ToUpper();

            Console.WriteLine(inputString);

            if(inputString == "QUIT")
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
    }
}
