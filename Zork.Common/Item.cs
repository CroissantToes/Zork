using System;
using System.Collections.Generic;
using System.Text;

namespace Zork.Common
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
