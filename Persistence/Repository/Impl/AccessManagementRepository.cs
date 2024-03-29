using AccessManagementService.Persistence.Entities;
using AccessManagementService.Persistence.Postgres;
using AutoFixture;

namespace AccessManagementService.Persistence.Repository.Impl;

public class AccessManagementRepository : IAccessManagementRepository
{
    private static readonly Fixture AutoFixture = new();

    private readonly AppDbContext _dbContext;

    public AccessManagementRepository(IServiceScopeFactory serviceScopeFactory)
    {
        var scope = serviceScopeFactory.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public Task<UserEntity?> FindUserByEmailAsync(string email)
    {
        var userEntity = AutoFixture.Build<UserEntity>()
            .With(user => user.Email, email)
            .With(user => user.BirthDate, DateTime.MinValue)
            .Create();

        return Task.FromResult(userEntity)!;
    }

    public Task<UserEntity> SaveUser(UserEntity userEntity)
    {
        var newUserEntity = new UserEntity
        {
            Id = AutoFixture.Create<string>(),
            Email = userEntity.Email,
            FullName = userEntity.FullName,
            Country = userEntity.Country,
            BirthDate = userEntity.BirthDate,
            Salary = userEntity.Salary,
            EmployerId = userEntity.EmployerId
        };

        return Task.FromResult(newUserEntity);
    }

    public Task RemoveUser(string email)
    {
        return Task.CompletedTask;
    }

    public Task<List<UserEntity>> FindUsersByEmployerName(string employerName)
    {
        var userEntities = AutoFixture.Build<UserEntity>()
            .CreateMany()
            .ToList();
        
        return Task.FromResult(userEntities);
    }

    public Task<EligibilityMetadataEntity> SaveEligibilityMetadataEntityAsync(EligibilityMetadataEntity eligibilityMetadataEntity)
    {
        var persistedEligibilityMetadataEntity = new EligibilityMetadataEntity
        {
            Id = Guid.NewGuid().ToString(),
            FileUrl = eligibilityMetadataEntity.FileUrl,
            EmployerName = eligibilityMetadataEntity.EmployerName
        };

        return Task.FromResult(persistedEligibilityMetadataEntity);
    }

    public Task<EligibilityMetadataEntity?> FindEligibilityMetadataEntityByEmployerName(string employerName)
    {
        var eligibilityMetadataEntity = AutoFixture.Build<EligibilityMetadataEntity>()
            .With(entity => entity.EmployerName, employerName)
            .Create();

        return Task.FromResult(eligibilityMetadataEntity)!;
    }
}