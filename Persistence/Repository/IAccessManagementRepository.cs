using AccessManagementService.Persistence.Entities;

namespace AccessManagementService.Persistence.Repository;

public interface IAccessManagementRepository
{
    Task<EligibilityMetadataEntity?> FindEligibilityMetadataEntityByEmployerNameAsync(string employerName);
    
    Task<EligibilityMetadataEntity> SaveEligibilityMetadataEntityAsync(EligibilityMetadataEntity eligibilityMetadataEntity);
}