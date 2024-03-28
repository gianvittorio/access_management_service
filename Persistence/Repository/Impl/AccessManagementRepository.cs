using AccessManagementService.Persistence.Entities;
using AutoFixture;

namespace AccessManagementService.Persistence.Repository.Impl;

public class AccessManagementRepository : IAccessManagementRepository
{
    private static readonly Fixture AutoFixture = new();

    public Task<UserEntity?> FindUserByEmailAsync(string email)
    {
        var userEntity = AutoFixture.Build<UserEntity>()
            .With(user => user.Email, email)
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
            EmployerName = userEntity.EmployerName
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
            .With(user => user.EmployerName, employerName)
            .CreateMany()
            .ToList();
        
        return Task.FromResult(userEntities);
    }

    public Task SaveEligibilityMetadataEntityAsync(EligibilityMetadataEntity eligibilityMetadataEntity)
    {
        return Task.CompletedTask;
    }

    public Task<EligibilityMetadataEntity> FindEligibilityMetadataEntityByEmployerName(string employerName)
    {
        var eligibilityMetadataEntity = AutoFixture.Build<EligibilityMetadataEntity>()
            .With(entity => entity.EmployerName, employerName)
            .Create();

        return Task.FromResult(eligibilityMetadataEntity);
    }
}