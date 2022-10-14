using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    internal class Player
    {
        public Room currentRoom
        {
            get
            {
                return _world.Rooms[_location.row, _location.column];
            }
        }

        public int Score { get; }

        public int Moves { get; }

        public Player(World world)
        {
            _world = world;
        }
        public bool Move(Commands command)
        {
            bool didMove;

            switch (command)
            {
                case Commands.North when _location.row < _world.Rooms.GetLength(0) - 1:
                    _location.row++;
                    didMove = true;
                    break;
                case Commands.South when _location.row > 0:
                    _location.row--;
                    didMove = true;
                    break;
                case Commands.East when _location.column < _world.Rooms.GetLength(1) - 1:
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

        private World _world;

        private static (int row, int column) _location = (1, 1);
    }
}
