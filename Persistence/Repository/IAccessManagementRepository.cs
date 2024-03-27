using AccessManagementService.Persistence.Entities;

namespace AccessManagementService.Persistence.Repository;

public interface IAccessManagementRepository
{
    Task SaveEligibilityEntity(EligibilityMetadataEntity eligibilityMetadataEntity);
    
    Task<EligibilityMetadataEntity> FindEligibilityMetadataEntityByEmployerName(string employerName);
}