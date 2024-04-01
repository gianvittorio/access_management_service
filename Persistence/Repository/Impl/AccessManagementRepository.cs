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

    public async Task<UserEntity?> FindUserByEmailAsync(string email)
    {
        return await CallInScope(async dbContext => await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email));
    }

    public async Task<UserEntity> SaveUser(UserEntity userEntity)
    {
        return await CallInScope(async dbContext =>
        {
            var persistedEntity = await dbContext.Users.FirstOrDefaultAsync(entity => entity.Email == userEntity.Email);
            if (persistedEntity is not null)
            {
                persistedEntity.FullName = userEntity.FullName;
                persistedEntity.Country = userEntity.Country;
                persistedEntity.BirthDate = userEntity.BirthDate;
                persistedEntity.Salary = userEntity.Salary;
                persistedEntity.EmployerId = userEntity.EmployerId;
            }
            else
            {
                persistedEntity = (await dbContext.Users.AddAsync(userEntity)).Entity;
            }

            return persistedEntity;
        });
    }

    public async Task<bool> RemoveUser(string email)
    {
        return await CallInScope(async dbContext =>
        {
            var userEntity = await dbContext.Users.FirstOrDefaultAsync(entity => entity.Email == email);
            if (userEntity is null)
            {
                return false;
            }
            
            dbContext.Users.Remove(userEntity);
            return true;
        });
    }

    public async Task<List<UserEntity>> FindUsersByEmployerName(string employerName)
    {
        return await CallInScope(async dbContext => await dbContext.EligibilityMetadata
            .Where(metadataEntity => metadataEntity.EmployerName == employerName)
            .SelectMany(metadataEntity => metadataEntity.Users)
            .ToListAsync());
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
                persistedEntity.Users = eligibilityMetadataEntity.Users;
            }
            else
            {
                persistedEntity = (await dbContext.EligibilityMetadata.AddAsync(eligibilityMetadataEntity)).Entity;
            }

            return persistedEntity;
        });
    }
}