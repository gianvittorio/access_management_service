using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Service.AccessManagement;

public interface IAccessManagementService
{
    Task<SelfSignupResult> SelfSignUpAsync(UserCredentials userCredentials);

    Task<FileProcessingResult> SaveEligibilityMetadataAsync(string fileUrl, string employerName);

    Task<FileProcessingResult> DownloadAndProcessEligibilityFileAsync(string fileUrl, string employerName);
}