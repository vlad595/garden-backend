using System;

namespace Models
{
    public abstract class CareResource : BaseEntity
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }

    public class Fertilizer : CareResource
    {
        public bool IsOrganic { get; set; }
        public string Nutrients { get; set; }
    }

    public class PestControl : CareResource
    {
        public string TargetPest { get; set; }
        public int WaitingDays { get; set; }
    }
}