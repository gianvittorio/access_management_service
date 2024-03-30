using AccessManagementService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessManagementService.Persistence.Postgres;

public class AppDbContext : DbContext
{
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<EligibilityMetadataEntity> EligibilityMetadata { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<UserEntity>()
           .HasOne(user => user.EligibilityMetadataEntity)
           .WithMany(metadata => metadata.Users)
           .HasForeignKey(user => user.EmployerId);
    }
}