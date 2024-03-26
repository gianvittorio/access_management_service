using AccessManagementService.Service.AccessManagement.Model;
using AccessManagementService.Service.UserFacade;
using AccessManagementService.Service.UserFacade.Dtos;

namespace AccessManagementService.Service.AccessManagement.Impl;

public class AccessManagementService : IAccessManagementService
{
    private readonly IUserServiceFacade _userServiceFacade;

    public AccessManagementService(IUserServiceFacade userServiceFacade)
    {
        _userServiceFacade = userServiceFacade;
    }

    public async Task<SelfSignupResult> SelfSignUp(string userEmail, string password, string country)
    {
        if (string.IsNullOrWhiteSpace(userEmail) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException();
        }

        var userResponseDto = await _userServiceFacade.FindUserByEmail(userEmail);
        if (userResponseDto is null)
        {
            var userRequestDto = new UserRequestDto
            {
                Email = userEmail,
                Password = password,
                AccessType = string.IsNullOrWhiteSpace(userResponseDto?.EmployerId) ? AccessType.Dtc : AccessType.Employer,
                Country = country
            };
            userResponseDto = await _userServiceFacade.CreateUser(userRequestDto);
        }
        
        var selfSignupResult = new SelfSignupResult
        {
            SignedIn = true,
            UserId = userResponseDto?.UserId,
            EmployerId = userResponseDto?.EmployerId
        };

        return selfSignupResult;
    }

    public Task<FileProcessingResult> DownloadAndProcessEligibilityFile(string fileUrl, string employerName)
    {
        var fileProcessingResult = new FileProcessingResult();
        
        return Task.FromResult(fileProcessingResult);
    }
}