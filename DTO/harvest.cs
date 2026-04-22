using System;
using Models;

namespace DTO
{
    public class HarvestBase
    {
        public DateTime HarvestDate {get; set;}
        public ProcessingMethods ProcessingMethod { get; set; }
        public double WeightKg {get; set;}
    }
    public class HarvestResponse : HarvestBase
    {
        public int Id {get; set;}
        public HarvestResponse(int id, DateTime harvestDate, ProcessingMethods processingMethod, double weightKg)
        {
            Id = id;
            HarvestDate = harvestDate;
            ProcessingMethod = processingMethod;
            WeightKg = weightKg;
        }
    }
    public class HarvestCreate : HarvestBase
    {
        public int PlantId {get; set;}
    }
}