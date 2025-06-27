using Microsoft.EntityFrameworkCore;
using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI
{
    public class PharmacyDbContext : DbContext
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<AlternativeMedicine> AlternativeMedicines { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<StockReplenishment> StockReplenishments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AlternativeMedicine: Self-referential many-to-many
            modelBuilder.Entity<AlternativeMedicine>()
                .HasOne(am => am.OriginalMedicine)
                .WithMany(m => m.Alternatives)
                .HasForeignKey(am => am.OriginalMedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AlternativeMedicine>()
                .HasOne(am => am.SubstituteMedicine)
                .WithMany()
                .HasForeignKey(am => am.SubstituteMedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            // StockReplenishment: Admin must exist
            modelBuilder.Entity<StockReplenishment>()
                .HasOne(sr => sr.AdminUser)
                .WithMany(u => u.Replenishments)
                .HasForeignKey(sr => sr.AdminUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
