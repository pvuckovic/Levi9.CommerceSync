using Levi9.CommerceSync.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Levi9.CommerceSync.Domain
{
    public class SyncDbContext : DbContext
    {
        public DbSet<SyncStatus> SyncStatuses { get; set; }
        public SyncDbContext(DbContextOptions<SyncDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SyncStatus>()
                .HasData(new SyncStatus
                {
                    Id = 1,
                    LastUpdate = "542389053214567843",
                    ResourceType = "PRODUCT"
                });
        }



    }
}
