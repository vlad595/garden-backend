using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options): base(options)
        {
            
        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<BerryBush> BerryBushes {get; set; }
        public DbSet<FruitTree> FruitTrees { get; set; }

        public DbSet<CareResource> CareResources { get; set; }
        public DbSet<Fertilizer> Fertilizers { get; set; }
        public DbSet<PestControl> PestControls { get; set; }

        public DbSet<Harvest> Harvests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}