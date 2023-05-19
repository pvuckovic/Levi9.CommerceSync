using Levi9.CommerceSync.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Levi9.CommerceSync.Domain
{
    public class SyncDbContext : DbContext
    {
        public DbSet<SyncStatus> SyncStatuses { get; set; }
        public SyncDbContext(DbContextOptions<SyncDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SyncStatus>()
                .HasData(new SyncStatus
                {
                    Id = 1,
                    LastUpdate = "000000000000000000",
                    ResourceType = "PRODUCT"
                });
        }



    }
}
