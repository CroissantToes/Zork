using System;
using System.Linq;
using Newtonsoft.Json;
using DominosNET.Stores;
using DominosNET.Customer;
using DominosNET.Address;
using DominosNET.Order;
using DominosNET.Payment;
using DominosNET.Menu;
using System.Net;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        [JsonIgnore]
        private bool IsOrdering { get; set; }

        [JsonIgnore]
        private Customer Customer { get; set; }
        [JsonIgnore]
        private Address Address { get; set; }
        [JsonIgnore]
        private Store Store { get; set; }
        [JsonIgnore]
        private Menu Menu { get; set; }
        [JsonIgnore]
        private Order Order { get; set; }


        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            Customer = new Customer("5555555555", "John", "Doe", "generic@email.com");
            Address = new Address("700 E Colonial Dr", "Orlando", "FL", "32803", "us", ServiceType.Delivery);
            Store = Address.closest_Store();
            Menu = Store.GetMenu();
            Order = new Order(Store, Customer, Address, "us");

            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            Output.WriteLine("Welcome to Zork!");
            Look();
            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string pizzaCommandFailed = "You need to be at Domino's Pizza to use this command.";
            bool isAtDominos = Player.CurrentRoom.Name.CompareTo("Domino's Pizza") == 0;

            string verb;
            string subject = null;
            if (commandTokens.Length == 0)
            {
                return;
            }
            else if (commandTokens.Length == 1)
            {
                verb = commandTokens[0];
            }
            else
            {
                verb = commandTokens[0];
                subject = commandTokens[1];
            }

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.Look:
                    Look();
                    Player.Moves++;
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    Player.Moves++;
                    break;

                case Commands.Score:
                    Output.WriteLine($"Your score would be {Player.Score}, in {Player.Moves + 1} move(s).");
                    Player.Moves++;
                    break;

                case Commands.Reward:
                    Player.Score++;
                    Output.WriteLine($"Your score has increased! Your new score is {Player.Score}.");
                    Player.Moves++;
                    break;

                case Commands.Take:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                    }
                    Player.Moves++;
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                    }
                    Player.Moves++;
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                        }
                    }
                    Player.Moves++;
                    break;
                case Commands.Talk:
                    if(isAtDominos == true && IsOrdering == false)
                    {
                        IsOrdering = true;
                        Output.WriteLine("'Welcome to Domino's. Take a look at the menu and search for what you would like to order,' says the dwarf.");
                    }
                    else if(Player.CurrentRoom.Name.CompareTo("Domino's Pizza") == 0 && IsOrdering == true)
                    {
                        Output.WriteLine("'Never ate at Domino's before? Use the search command to see if the menu has what you want." +
                            "\nUse the add command and a product code to add an item to your order.\nUse the remove command to remove an item." +
                            "\n Use the purchase command when you are satisfied with your order to finalize it.' says the dwarf.");
                    }
                    else
                    {
                        Output.WriteLine(pizzaCommandFailed);
                    }
                    Player.Moves++;
                    break;
                case Commands.Search:
                    if(isAtDominos == true)
                    {
                        if (string.IsNullOrEmpty(subject))
                        {
                            Output.WriteLine("This command requires a subject.");
                        }
                        else
                        {
                            Menu.Search(subject);
                        }
                    }
                    else
                    {
                        Output.WriteLine(pizzaCommandFailed);
                    }
                    Player.Moves++;
                    break;
                case Commands.Add:
                    if (isAtDominos == true)
                    {
                        if (string.IsNullOrEmpty(subject))
                        {
                            Output.WriteLine("This command requires a subject. The subject must be a product code.");
                        }
                        else
                        {
                            Order.add_item(1, subject);
                        }
                    }
                    else
                    {
                        Output.WriteLine(pizzaCommandFailed);
                    }
                    break;
                case Commands.Remove:
                    if (isAtDominos == true)
                    {
                        if (string.IsNullOrEmpty(subject))
                        {
                            Output.WriteLine("This command requires a subject. The subject must be a product code.");
                        }
                        else
                        {
                            Order.remove_item(1, subject);
                        }
                    }
                    else
                    {
                        Output.WriteLine(pizzaCommandFailed);
                    }
                    break;
                case Commands.Order:
                    if (isAtDominos == true)
                    {
                        Order.view_order();
                    }
                    else
                    {
                        Output.WriteLine(pizzaCommandFailed);
                    }
                    break;
                case Commands.Purchase:
                    if (isAtDominos == true)
                    {
                        Order.print_receipt();

                        string[] orderNames = Order.get_order_names();

                        foreach(string name in orderNames)
                        {
                            Player.AddItemToInventory(new Item(name, $"A {name} lies on the ground.", $"A {name}."));
                        }

                        Order = new Order(Store, Customer, Address, "us");
                    }
                    else
                    {
                        Output.WriteLine(pizzaCommandFailed);
                    }
                    break;
                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            

            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }

            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
        }

        private void Take(string itemName)
        {
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Output.WriteLine("Taken.");
            }
        }

        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Output.WriteLine("Dropped.");
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}