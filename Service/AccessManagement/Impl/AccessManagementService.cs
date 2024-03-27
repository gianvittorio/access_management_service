using AccessManagementService.Domain.Core.Lib.PasswordValidation.Impl;
using AccessManagementService.Persistence.Entities;
using AccessManagementService.Persistence.Repository;
using AccessManagementService.Service.AccessManagement.Model;
using AccessManagementService.Service.EmployerFacade;
using AccessManagementService.Service.UserFacade;
using AccessManagementService.Service.UserFacade.Dtos;
using AutoFixture;

namespace AccessManagementService.Service.AccessManagement.Impl;

public class AccessManagementService : IAccessManagementService
{
    private static readonly Fixture AutoFixture = new();
    private readonly IUserServiceFacade _userServiceFacade;
    private readonly IEmployerServiceFacade _employerServiceFacade;
    private readonly IAccessManagementRepository _accessManagementRepository;

    public AccessManagementService(
        IUserServiceFacade userServiceFacade, 
        IAccessManagementRepository accessManagementRepository, 
        IEmployerServiceFacade employerServiceFacade
        )
    {
        _userServiceFacade = userServiceFacade;
        _accessManagementRepository = accessManagementRepository;
        _employerServiceFacade = employerServiceFacade;
    }

    public async Task<SelfSignupResult> SelfSignUpAsync(string userEmail, string password, string country, string employerName)
    {
        if (string.IsNullOrWhiteSpace(userEmail) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException();
        }

        if (!IsPasswordValid(password))
        {
            throw new ArgumentException();
        }

        User? employeeUser = null;
        if (!string.IsNullOrWhiteSpace(employerName))
        {
            var eligibilityMetadataForEmployerName = await _accessManagementRepository.FindEligibilityMetadataEntityByEmployerName(employerName);
            var fileProcessingResult = await DownloadAndProcessEligibilityFileAsync(eligibilityMetadataForEmployerName.FileUrl, employerName);
            employeeUser = FindRegisteredUserByEmail(userEmail, fileProcessingResult);
        }
        
        UserResponseDto? userResponseDto;
        if (employeeUser is not null)
        {
            var userRequestDto = new UserRequestDto
            {
                Email = userEmail,
                Password = password,
                Country = employeeUser.Country,
                AccessType = UserAccessType.Employer,
                FullName = employeeUser.FullName,
                BirthDate = employeeUser.BirthDate,
                Salary = employeeUser.Salary,
                EmployerId = await _employerServiceFacade.FindEmployerIdByEmployerName(employerName)
            };
            userResponseDto = await _userServiceFacade.SaveUserAsync(userRequestDto);
        }
        else
        {
            userResponseDto = await _userServiceFacade.FindUserByEmailAsync(userEmail);
            if (userResponseDto is null)
            {
                var userRequestDto = new UserRequestDto
                {
                    Email = userEmail,
                    Password = password,
                    Country = country,
                    AccessType = UserAccessType.Dtc,
                };
                
                userResponseDto = await _userServiceFacade.SaveUserAsync(userRequestDto);
            }
        }
        
        var selfSignupResult = new SelfSignupResult
        {
            UserId = userResponseDto.UserId,
            UserAccessType = userResponseDto.AccessType
        };

        return selfSignupResult;
    }

    public async Task<FileProcessingResult> SaveEligibilityMetadataAsync(string fileUrl, string employerName)
    {
        var fileProcessingResult = await DownloadAndProcessEligibilityFileAsync(fileUrl, employerName);
        
        var eligibilityMetadataEntity = new EligibilityMetadataEntity
        {
            FileUrl = fileUrl,
            EmployerName = employerName
        };
        await _accessManagementRepository.SaveEligibilityMetadataEntityAsync(eligibilityMetadataEntity);

        return fileProcessingResult;
    }

    public Task<FileProcessingResult> DownloadAndProcessEligibilityFileAsync(string fileUrl, string employerName)
    {
        // To do: Process line by line, updating each and and every user's country and salary, as well as terminating user's
        // accounts no longer listed in eligibility file
        
        var fileProcessingResult = AutoFixture.Build<FileProcessingResult>()
            .With(result => result.EmployerName, employerName)
            .Create();
        
        return Task.FromResult(fileProcessingResult);
    }

    private User? FindRegisteredUserByEmail(string email, FileProcessingResult fileProcessingResult)
    {
        return AutoFixture.Build<User>()
            .With(user => user.Email, email)
            .Create();
    }

    private bool IsPasswordValid(string password)
    {
        return new PasswordValidator().Validate(password);
    }
}