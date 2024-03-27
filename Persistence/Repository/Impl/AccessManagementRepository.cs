using AccessManagementService.Persistence.Entities;
using AutoFixture;

namespace AccessManagementService.Persistence.Repository.Impl;

public class AccessManagementRepository : IAccessManagementRepository
{
    private static readonly Fixture AutoFixture = new();
    
    public Task SaveEligibilityEntity(EligibilityMetadataEntity eligibilityMetadataEntity)
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