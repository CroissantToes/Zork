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
            Output.WriteLine($"{Player.currentRoom.Description}");
            foreach (Item item in Player.currentRoom.Inventory)
            {
                Output.WriteLine($"{item.Description}");
            }
            Player.currentRoom.HasBeenVisited = true;
            Output.Write("\n> ");
        }

        private void MakeMove(object sender, string inputString)
        {
            Room previousRoom = Player.currentRoom;

            var splitInput = inputString.Split(" ");
            Commands command = ToCommand(splitInput[0]);
            string outputString;

            switch (command)
            {
                case Commands.Quit:
                    outputString = "Thank you for playing!";
                    IsRunning = false;
                    break;
                case Commands.Look:
                    outputString = Player.currentRoom.Description;
                    foreach(Item item in Player.currentRoom.Inventory)
                    {
                        outputString += $"\n{item.Description}";
                    }
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
                case Commands.Take:
                    if(splitInput.Length > 1)
                    {
                        outputString = Player.Take(splitInput[1]);
                    }
                    else
                    {
                        outputString = "This command requires a subject.";
                    }
                    break;
                case Commands.Drop:
                    if(splitInput.Length > 1)
                    {
                        outputString= Player.Drop(splitInput[1]);
                    }
                    else
                    {
                        outputString = "This command requires a subject.";
                    }
                    break;
                case Commands.Inventory:
                    outputString = PrintInventory();
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

                if (previousRoom != Player.currentRoom && !Player.currentRoom.HasBeenVisited)
                {
                    Output.WriteLine($"{Player.currentRoom}\n{Player.currentRoom.Description}\n");
                    previousRoom = Player.currentRoom;
                    Player.currentRoom.HasBeenVisited = true;
                }
                else
                {
                    Output.WriteLine($"\n{Player.currentRoom}");
                }

                Output.Write("> ");
            }
        }

        private string PrintInventory()
        {
            string output = "";

            if(Player.Inventory.Count > 0)
            {
                foreach(Item item in Player.Inventory)
                {
                    if(Player.Inventory.IndexOf(item) < Player.Inventory.Count - 1)
                    {
                        output += $"{item.Description}\n";
                    }
                    else
                    {
                        output += $"{item.Description}";
                    }
                }
            }
            else
            {
                output = "You are empty handed.";
            }

            return output;
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
