using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Zork.Common
{
    public class Player
    {
        public Room currentRoom
        {
            get => _currentRoom;
            set => _currentRoom = value;
        }

        public List<Item> Inventory { get; }

        public int Score { get; set; }

        public int Moves { get; set; }

        public Player(World world, string startingLocation)
        {
            _world = world;

            if(_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            Inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);

            if(didMove)
            {
                currentRoom = neighbor;
            }

            return didMove;
        }

        public bool AddItemToInventory(Item itemToAdd)
        {
            return false;
        }

        public string Take(string itemToTake)
        {
            string outcome;

            if(currentRoom.Inventory.Exists(item => String.Compare(item.Name, itemToTake, ignoreCase: true) == 0))
            {
                var subject = currentRoom.Inventory.Find(item => String.Compare(item.Name, itemToTake, ignoreCase: true) == 0);
                Inventory.Add(subject);
                currentRoom.Inventory.Remove(subject);
                outcome = "Taken.";
            }
            else
            {
                outcome = "You can't see any such thing.";
            }

            return outcome;
        }

        public string Drop(string itemToDrop)
        {
            string outcome;

            if (Inventory.Exists(item => String.Compare(item.Name, itemToDrop, ignoreCase: true) == 0))
            {
                var subject = Inventory.Find(item => String.Compare(item.Name, itemToDrop, ignoreCase: true) == 0);
                currentRoom.Inventory.Add(subject);
                Inventory.Remove(subject);
                outcome = "Dropped.";
            }
            else
            {
                outcome = "You don't have any such thing.";
            }

            return outcome;
        }

        private World _world;

        private Room _currentRoom;
    }
}
