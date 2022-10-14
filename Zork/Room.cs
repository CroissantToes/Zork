using Newtonsoft.Json;

namespace Zork
{
    internal class Room
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool HasBeenVisited { get; set; }

        public Room(string name)
        {
            Name = name;
        }

        [JsonConstructor]
        public Room(string name, string description = null)
        {
            Name = name;
            Description = description;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}