using System;

namespace Models
{
    public class Plant : BaseEntity
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public int age {get; set;}
        public PlantStatus Status { get; set; }
        public DateTime PlantedAt { get; set; }
        public List<Harvest> harvests { get; set; } = new List<Harvest>();
    }

    public class FruitTree : Plant
    {
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