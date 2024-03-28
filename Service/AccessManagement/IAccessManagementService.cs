using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Service.AccessManagement;

public interface IAccessManagementService
{
    Task<SelfSignupResult> SelfSignUpAsync(string userEmail, string password, string country, string? employerName = null);

    Task<FileProcessingResult> SaveEligibilityMetadataAsync(string fileUrl, string employerName);

    Task<FileProcessingResult> DownloadAndProcessEligibilityFileAsync(string fileUrl, string employerName);
}