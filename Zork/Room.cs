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
        public Room(string name, string description = null)
        {
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}