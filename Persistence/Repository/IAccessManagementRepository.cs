using AccessManagementService.Persistence.Entities;

namespace AccessManagementService.Persistence.Repository;

public interface IAccessManagementRepository
{
    Task SaveEligibilityMetadataEntityAsync(EligibilityMetadataEntity eligibilityMetadataEntity);
    
    Task<EligibilityMetadataEntity> FindEligibilityMetadataEntityByEmployerName(string employerName);
}