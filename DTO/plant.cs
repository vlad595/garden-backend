using System;

namespace DTO
{
    public class PlantBase
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public DateTime PlantedAt { get; set; }
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