using System;

namespace DTO
{
    public class GardenStatisticsDto
    {
        // 1. Статистика садку
        public int TotalPlants { get; set; }
        public int SickPlants { get; set; }
        public int TreatedPlants { get; set; }

        // 2. Статистика плодів (врожаю)
        public double TotalHarvestWeightLastMonthKg { get; set; }
        public int HarvestsLastMonthCount { get; set; }
        public double FrozenFruitsWeightKg { get; set; }
        public double SoldFruitsWeightLastMonthKg { get; set; }
        public decimal TotalMonthlyProfit { get; set; } 
        public double FruitsForConservationWeightKg { get; set; }
        public string MostFruitfulPlantName { get; set; }

        // 3. Статистика засобів
        public int TotalCareProducts { get; set; }
        public int FertilizerCount { get; set; }
        public int PestControlCount { get; set; }
        public decimal TotalExpenses { get; set; }
    }
}