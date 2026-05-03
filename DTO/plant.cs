using System;
using Models;

namespace DTO
{
    public class PlantBase
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public DateTime PlantedAt { get; set; }
        public PlantStatus Status { get; set; }
    }

    public class PlantResponse : PlantBase
    {
        public int Id {get; set;}
        public string Type {get; set;}
    }

    public class FruitTreeCreation : PlantBase
    {
        public double Height {get; set;}
    }

    public class BerryBushCreation : PlantBase
    {
        public bool TrellisNeeds {get; set;}
    } 
}