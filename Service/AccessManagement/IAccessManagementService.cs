using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Service.AccessManagement;

public interface IAccessManagementService
{
    Task<SelfSignupResult> SelfSignUp(string userEmail, string password, string country);

    Task<FileProcessingResult> DownloadAndProcessEligibilityFile(string fileUrl, string employerName);
}