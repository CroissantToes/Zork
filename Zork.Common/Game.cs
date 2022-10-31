using System;
using Newtonsoft.Json;
using System.IO;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public IInputService Input { get; private set; }

        public IOutputService Output { get; private set; }

        public bool IsRunning { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(world, startingLocation);
        }

        public void Run(string[] args, IOutputService output, IInputService input)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            Input.InputReceived += MakeMove;

            IsRunning = true;

            Output.WriteLine(Player.currentRoom);
            Output.WriteLine($"{Player.currentRoom.Description}\n");
            Player.currentRoom.HasBeenVisited = true;
            Output.Write("> ");
        }

        private void MakeMove(object sender, string inputString)
        {
            Room previousRoom = null;

            Commands command = ToCommand(inputString);

            string outputString;
            switch (command)
            {
                case Commands.Quit:
                    outputString = "Thank you for playing!";
                    IsRunning = false;
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

            Output.WriteLine(outputString);

            if (command != Commands.Quit)
            {
                Player.Moves++;
                
                Output.WriteLine(Player.currentRoom);

                if (previousRoom != Player.currentRoom && !Player.currentRoom.HasBeenVisited)
                {
                    Output.WriteLine(Player.currentRoom.Description);
                    previousRoom = Player.currentRoom;
                    Player.currentRoom.HasBeenVisited = true;
                }
                Output.WriteLine(" ");
                Output.Write("> ");
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
