using AccessManagementService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessManagementService.Persistence.Postgres;

public class AppDbContext : DbContext
{
    public DbSet<EligibilityMetadataEntity> EligibilityMetadata { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}