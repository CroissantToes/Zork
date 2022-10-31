﻿using System;
using System.Collections.Generic;

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

        private void Take()
        {

        }

        private World _world;

        private Room _currentRoom;
    }
}