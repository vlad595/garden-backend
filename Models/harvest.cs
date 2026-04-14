using System;

namespace Models
{
    public class Harvest : BaseEntity
    {
        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;
        public double WeightKg { get; set; }
        public DateTime HarvestDate { get; set; }
        public ProcessingMethods ProcessingMethod { get; set; }
    }

    public enum ProcessingMethods
    {
        Sell,
        Conservation,
        Freezing
    }
}