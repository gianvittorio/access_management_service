using AccessManagementService.Persistence.Entities;

namespace AccessManagementService.Persistence.Repository;

public interface IAccessManagementRepository
{
    Task<UserEntity?> FindUserByEmailAsync(string email);

    Task<UserEntity> SaveUserAsync(UserEntity userEntity);
    
    Task<UserEntity?> UpdateUserCountryAndSalaryIfExistsAsync(string email, string country, decimal salary);

    Task<bool> RemoveUserAsync(string email);
    
    Task<List<UserEntity>> FindUsersByEmployerNameAsync(string employerName);
    
    Task<EligibilityMetadataEntity?> FindEligibilityMetadataEntityByEmployerNameAsync(string employerName);
    
    Task<EligibilityMetadataEntity> SaveEligibilityMetadataEntityAsync(EligibilityMetadataEntity eligibilityMetadataEntity);
}