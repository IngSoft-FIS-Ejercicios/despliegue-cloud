using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;

namespace PharmaGo.DataAccess
{
    public class PharmacyGoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<StockRequest> StockRequests { get; set; }
        public DbSet<StockRequestDetail> StockRequestDetails { get; set; }
        public DbSet<UnitMeasure> UnitMeasures { get; set; }
        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Cosmetic> Cosmetics { get; set; }
        public DbSet<Product> Products { get; set; }

        public PharmacyGoDbContext(DbContextOptions<PharmacyGoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");

            modelBuilder.Entity<Drug>().ToTable("Drugs");

            modelBuilder.Entity<Cosmetic>().ToTable("Cosmetics");

            modelBuilder.Entity<Drug>().HasBaseType<Product>();  
            modelBuilder.Entity<Cosmetic>().HasBaseType<Product>();  

            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(14, 2); 

            modelBuilder.Entity<Purchase>().Property(p => p.TotalAmount).HasPrecision(14, 2);
            modelBuilder.Entity<PurchaseDetail>().Property(pd => pd.Price).HasPrecision(14, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
