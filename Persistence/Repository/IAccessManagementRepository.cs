using AccessManagementService.Persistence.Entities;

namespace AccessManagementService.Persistence.Repository;

public interface IAccessManagementRepository
{
    Task<UserEntity?> FindUserByEmailAsync(string email);

    Task<UserEntity> SaveUser(UserEntity userEntity);

    Task<bool> RemoveUser(string email);
    
    Task<List<UserEntity>> FindUsersByEmployerName(string employerName);
    
    Task<EligibilityMetadataEntity?> FindEligibilityMetadataEntityByEmployerNameAsync(string employerName);
    
    Task<EligibilityMetadataEntity> SaveEligibilityMetadataEntityAsync(EligibilityMetadataEntity eligibilityMetadataEntity);
}