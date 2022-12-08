using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Player
    {
        //public event EventHandler<Room> locationChanged;
        //public event EventHandler<int> scoreChanged;
        //public event EventHandler<int> movesChanged;

        public int Score 
        { 
            get => _score; 
            set
            {
                _score = value;
                //scoreChanged.Invoke(this, _score);
            }
        }
        public int Moves
        {
            get => _moves;
            set
            {
                _moves = value;
                //movesChanged.Invoke(this, _moves);
            }
        }

        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                if(_currentRoom != value)
                {
                    _currentRoom = value;
                    //locationChanged?.Invoke(this, _currentRoom);
                }
            }
        }

        public IEnumerable<Item> Inventory => _inventory;

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            _inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
            }

            return didMove;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_inventory.Contains(itemToAdd))
            {
                throw new Exception($"Item {itemToAdd} already exists in inventory.");
            }

            _inventory.Add(itemToAdd);
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
        }

        private readonly World _world;
        private Room _currentRoom;
        private int _score;
        private int _moves;
        private readonly List<Item> _inventory;
    }
}
