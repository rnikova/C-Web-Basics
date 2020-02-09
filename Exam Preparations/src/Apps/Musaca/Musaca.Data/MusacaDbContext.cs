using Microsoft.EntityFrameworkCore;
using Musaca.Models;

namespace Musaca.Data
{
    public class MusacaDbContext : DbContext
    {
        public DbSet<User> Users {get; set;}

        public DbSet<Product> Products {get; set;}

        public DbSet<Order> Orders {get; set;}

        public DbSet<OrderProduct> OrdersProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbSettings.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<Product>()
                .HasKey(product => product.Id);

            modelBuilder.Entity<Order>()
                .HasKey(order => order.Id);

            modelBuilder.Entity<Order>()
                .HasMany(order => order.Products)
                .WithOne(orderProduct => orderProduct.Order)
                .HasForeignKey(order => order.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Cashier);

            modelBuilder.Entity<OrderProduct>()
               .HasKey(orderProduct => new { orderProduct.OrderId, orderProduct.ProductId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
