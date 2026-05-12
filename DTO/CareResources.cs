using System;

namespace DTO
{
    public class CreateFertilizerDto
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsOrganic { get; set; }
        public string Nutrients { get; set; }
    }

    public class CreatePestControlDto
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public string TargetPest { get; set; }
        public int WaitingDays { get; set; }
    }

    public class UpdateQuantityDto
    {
        public double Quantity { get; set; }
    }

    public class CareResourceResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ResourceType { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public bool? IsOrganic { get; set; }
        public string Nutrients { get; set; }
        public string TargetPest { get; set; }
        public int? WaitingDays { get; set; }
    }
}