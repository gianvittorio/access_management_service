using AccessManagementService.Persistence.Entities;
using AccessManagementService.Persistence.Postgres;
using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace AccessManagementService.Persistence.Repository.Impl;

public class AccessManagementRepository : IAccessManagementRepository
{
    private static readonly Fixture AutoFixture = new();

    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public AccessManagementRepository(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    private async Task<T> CallInScope<T>(Func<AppDbContext, Task<T>> func)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        try
        {
            await dbContext.Database.OpenConnectionAsync();
            await dbContext.Database.EnsureCreatedAsync();
            var result = await func(dbContext);
            await dbContext.SaveChangesAsync();

            return result;
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }
    }

    public Task<bool> RemoveUserAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<EligibilityMetadataEntity?> FindEligibilityMetadataEntityByEmployerNameAsync(string employerName)
    {
        return await CallInScope(async dbContext => await dbContext.EligibilityMetadata
            .FirstOrDefaultAsync(metadataEntity => metadataEntity.EmployerName == employerName));
    }
    
    public async Task<EligibilityMetadataEntity> SaveEligibilityMetadataEntityAsync(EligibilityMetadataEntity eligibilityMetadataEntity)
    {
        return await CallInScope(async dbContext =>
        {
            var persistedEntity = await dbContext.EligibilityMetadata.FirstOrDefaultAsync(entity => entity.EmployerName == eligibilityMetadataEntity.EmployerName);
            if (persistedEntity is not null)
            {
                persistedEntity.FileUrl = eligibilityMetadataEntity.FileUrl;
            }
            else
            {
                persistedEntity = (await dbContext.EligibilityMetadata.AddAsync(eligibilityMetadataEntity)).Entity;
            }

            return persistedEntity;
        });
    }
}