using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Report;
using Microsoft.EntityFrameworkCore;

namespace Martins.Backend.Infrastructure.Repository.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Material> Material { get; set; }
        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductMaterial> ProductMaterial { get; set; }
        public DbSet<ProductAdditionalCost> ProductAdditionalCost { get; set; }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<OperationalExpense> OperationalExpense { get; set; }
        public DbSet<OrderAdditionalCost> OrderAdditionalCost { get; set; }


        public DbSet<ReportMaterial> ReportMaterial { get; set; }
        public DbSet<ReportProduct> ReportProduct { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Material>()
                .Property(m => m.UnitCost)
                .HasColumnType("decimal(18, 4)");
        }
    }
}
