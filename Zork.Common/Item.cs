using System;
using System.Collections.Generic;
using System.Text;

namespace Zork.Common
{
    public class Item
    {
        public string Name { get; set; }
        private string Description { get; }

        private string[] Aliases;

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
