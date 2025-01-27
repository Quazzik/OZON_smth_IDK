using Microsoft.EntityFrameworkCore;
using OZON_Delivery_checker.DataBase;

namespace OZON_Delivery_checker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<RequestRecord> RequestRecords { get; set; }

        public DbSet<RequestEvent> RequestEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RequestEvent>()
                .HasOne(e => e.RequestRecord)
                .WithMany()
                .HasForeignKey(e => e.RequestRecordId);
        }
    }
}
