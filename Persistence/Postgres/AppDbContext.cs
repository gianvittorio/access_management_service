using AccessManagementService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessManagementService.Persistence.Postgres;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<EligibilityMetadataEntity> EligibilityMetadata { get; set; }
}