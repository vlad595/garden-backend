using System;
using DTO;

namespace Models
{
    public class Plant : BaseEntity
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int Age {get; set;}
        public PlantStatus Status { get; set; }
        public DateTime PlantedAt { get; set; }
        public List<Harvest> harvests { get; set; } = new List<Harvest>();
    }

    public class FruitTree : Plant
    {

        public FruitTree()
        {
            
        }

        public FruitTree(string name, string species, DateTime plantedAt, double height)
        {
            this.Name = name;
            this.Species = species;
            this.PlantedAt = plantedAt;
            this.Height = height;
        }
        public double Height { get; set; }
    }

    public class BerryBush : Plant
    {
        public bool TrellisNeeds {get; set;}
    }

    public enum PlantStatus
    {
        Healthy,
        Sick,
        Treated,
        Dead
    }
}