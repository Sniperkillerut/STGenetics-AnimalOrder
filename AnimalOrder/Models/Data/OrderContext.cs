using Microsoft.EntityFrameworkCore;

namespace AnimalOrder.Models.Data
{
    public class OrderContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OrderAnimal>().HasKey(oa => new { oa.OrderId, oa.AnimalId });

            modelBuilder
                .Entity<Animal>()
                .Property(e => e.Sex)
                .HasConversion<int>();
        }
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }
 
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderAnimal> OrderAnimals { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
