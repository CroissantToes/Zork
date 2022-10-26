using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Room
    {
        [JsonProperty(Order = 1)]
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string Description { get; set; }

        public bool HasBeenVisited { get; set; }

        [JsonIgnore]
        public Dictionary<Directions, Room> Neighbors { get; private set; }

        [JsonProperty(PropertyName = "Neighbors", Order = 3)]
        private Dictionary<Directions, string> NeighborNames { get; set; }

        [JsonIgnore]
        public List<Item> Inventory { get; private set; }

        [JsonProperty(PropertyName = "Inventory")]
        private string[] InventoryNames { get; }

        [JsonConstructor]
        public Room(string name, string description, Dictionary<Directions, string> neighborNames, string[] inventoryNames)
        {
            Name = name;
            Description = description;
            NeighborNames = neighborNames ?? new Dictionary<Directions, string>();
            InventoryNames = inventoryNames ?? new string[0];
        }

        public void UpdateNeighbors(World world)
        {
            Neighbors = new Dictionary<Directions, Room>();
            foreach(KeyValuePair<Directions, string> neighborName in NeighborNames)
            {
                Neighbors.Add(neighborName.Key, world.RoomsByName[neighborName.Value]);
            }

            NeighborNames = null;
        }

        public void UpdateInventory(World world)
        {
            Inventory = new List<Item>();
            foreach(var inventoryName in InventoryNames)
            {
                Inventory.Add(world.ItemsByName[inventoryName]);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}