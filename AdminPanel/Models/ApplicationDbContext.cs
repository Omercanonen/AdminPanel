using AdminPanel.Models.Customer;
using AdminPanel.Models.Employees;
using AdminPanel.Models.Products;
using AdminPanel.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceEntity>()
        .HasOne(s => s.Customer)
        .WithMany(c => c.Services)
        .HasForeignKey(s => s.CustomerId)
        .OnDelete(DeleteBehavior.NoAction); // NO ACTION ekleyin

            modelBuilder.Entity<ServiceEntity>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Services)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION ekleyin

            modelBuilder.Entity<ServiceEntity>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Services) 
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // NO ACTION ekleyin

            //modelBuilder.Entity<ProductModel>()
            //    .HasOne(pm => pm.Products)
            //    .WithMany(p => p.ProductModels)
            //    .HasForeignKey(pm => pm.ProductId)
            //    .OnDelete(DeleteBehavior.NoAction); // NO ACTION ekleyin
        }
    }
}

