using System;
using System.Collections.Generic;
using System.Text;

namespace Zork
{
    internal class World
    {
        public Room[,] Rooms
        {
            get
            {
                return _rooms;
            }
        }

        private static Room[,] _rooms;
    }
}
