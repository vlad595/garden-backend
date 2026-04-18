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

        public int UserId {get;set;}
        public User User {get;set;} = null!;

        public List<Harvest> Harvests { get; set; } = new List<Harvest>();
    }

    public class FruitTree : Plant
    {

        public FruitTree()
        {
            
        }

        public FruitTree(string name, string species, DateTime plantedAt, double height, int UserId)
        {
            this.Name = name;
            this.Species = species;
            this.PlantedAt = plantedAt;
            this.Height = height;
            this.UserId = UserId;
        }
        public double Height { get; set; }
    }

    public class BerryBush : Plant
    {
        public BerryBush(string name, string species, DateTime plantedAt, bool trellisNeeds, int UserId)
        {
            this.Name = name;
            this.Species = species;
            this.PlantedAt = plantedAt;
            this.TrellisNeeds = trellisNeeds;
            this.UserId = UserId;
        }
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