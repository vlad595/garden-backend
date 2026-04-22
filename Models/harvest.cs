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

        public Harvest(int plantId, double weightKg, DateTime harvestDate, ProcessingMethods processingMethod) {
            PlantId = plantId;
            WeightKg = weightKg;
            HarvestDate = harvestDate;
            ProcessingMethod = processingMethod;
        }
    }

    public enum ProcessingMethods
    {
        Sell,
        Conservation,
        Freezing
    }
}