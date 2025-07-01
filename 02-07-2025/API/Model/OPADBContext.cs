﻿using Microsoft.EntityFrameworkCore;
namespace OnlinePharmacyAppAPI.Model
{
    public class OPADBContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {

            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var configSection = configBuilder.GetSection("ConnectionStrings");

            var connectionString = configSection["DefaultConnection"] ?? null;

            optionsBuilder.UseSqlServer(connectionString);

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<StockReplenishment> stocks{ get; set; }
       
        public DbSet<Profile> profiles { get; set; }
        public DbSet<Cart> Cart { get; set; }

        public OPADBContext(DbContextOptions opts) : base(opts)

        {

        }
       

    }
}
