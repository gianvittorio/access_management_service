using System.Globalization;
using AccessManagementService.Domain.Core.Lib.CsvFileProcessing.Impl;
using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
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
    private readonly HttpClient _httpClient;
    private readonly IUserServiceFacade _userServiceFacade;
    private readonly IEmployerServiceFacade _employerServiceFacade;
    private readonly IAccessManagementRepository _accessManagementRepository;

    public AccessManagementService(
        IUserServiceFacade userServiceFacade, 
        IAccessManagementRepository accessManagementRepository, 
        IEmployerServiceFacade employerServiceFacade, 
        HttpClient httpClient
        )
    {
        _userServiceFacade = userServiceFacade;
        _accessManagementRepository = accessManagementRepository;
        _employerServiceFacade = employerServiceFacade;
        _httpClient = httpClient;
    }

    public async Task<SelfSignupResult> SelfSignUpAsync(UserCredentials userCredentials)
    {
        EligibilityMetadataEntity? eligibilityMetadataForEmployerName = null;
        User? employeeUser = null;
        if (!string.IsNullOrWhiteSpace(userCredentials.EmployerName))
        {
            eligibilityMetadataForEmployerName = await _accessManagementRepository.FindEligibilityMetadataEntityByEmployerNameAsync(userCredentials.EmployerName);
            var fileProcessingResult = await DownloadAndProcessEligibilityFileAsync(eligibilityMetadataForEmployerName.FileUrl, userCredentials.EmployerName);
            employeeUser = FindRegisteredUserByEmail(userCredentials.Email, fileProcessingResult);
        }
        
        UserAccessType userAccessType;
        string? employerId = null;
        if (employeeUser is not null)
        {
            userAccessType = UserAccessType.Employer;
            employerId = await _employerServiceFacade.FindEmployerIdByEmployerName(userCredentials.EmployerName!);
        }
        else
        {
            userAccessType = UserAccessType.Dtc;
        }
        
        var userEntity = new UserEntity
        {
            Email = userCredentials.Email,
            FullName = userCredentials.FullName,
            Country = userCredentials.Country,
            BirthDate = DateTime.Parse(userCredentials.BirthDate, DateTimeFormatInfo.InvariantInfo),
            Salary = userCredentials.Salary,
            EmployerId = eligibilityMetadataForEmployerName?.EmployerId
        };
        var persistedUserEntity = await _accessManagementRepository.SaveUser(userEntity);
        
        var userRequestDto = new UserRequestDto
        {
            Email = userCredentials.Email,
            Password = userCredentials.Password,
            Country = userCredentials.Country,
            AccessType = userAccessType.ToString(),
            FullName = userCredentials.FullName,
            BirthDate = DateTime.Parse(userCredentials.BirthDate, CultureInfo.InvariantCulture),
            Salary = userCredentials.Salary,
            EmployerId = employerId
        };
        _ = await _userServiceFacade.SaveUserAsync(userRequestDto);
        
        var selfSignupResult = new SelfSignupResult
        {
            UserId = persistedUserEntity.Id,
            UserAccessType = userAccessType
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

    public async Task<FileProcessingResult> DownloadAndProcessEligibilityFileAsync(string fileUrl, string employerName)
    {
        var eligibilityMetadataEntity = new EligibilityMetadataEntity
        {
            FileUrl = fileUrl,
            EmployerName = employerName
        };
        var persistedEligibilityMetadataEntity = await _accessManagementRepository.FindEligibilityMetadataEntityByEmployerNameAsync(employerName)
                                                 ?? await _accessManagementRepository.SaveEligibilityMetadataEntityAsync(eligibilityMetadataEntity);
        using var response = await _httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
        await using var responseContentStream = await response.Content.ReadAsStreamAsync();
        using var streamReader = new StreamReader(responseContentStream);
        var eligibilityFileStreamProcessor = new EligibilityFileStreamProcessor(employerName);
        var fileProcessingResult = await eligibilityFileStreamProcessor.Process(streamReader);

        foreach (var user in fileProcessingResult.Users)
        {
            var userEntity = new UserEntity
            {
                Email = user.Email,
                FullName = user.FullName,
                Country = user.Country,
                BirthDate = DateTime.Parse(user.BirthDate, DateTimeFormatInfo.CurrentInfo).ToUniversalTime(),
                Salary = user.Salary,
                EmployerId = persistedEligibilityMetadataEntity.EmployerId
            };
            await _accessManagementRepository.SaveUser(userEntity);
        }
        
        var usersForEmployerName = (await _accessManagementRepository.FindUsersByEmployerName(employerName));
        var usersToBeRemoved = usersForEmployerName
            .ExceptBy(fileProcessingResult.Users.Select(user => user.Email), user => user.Email);

        foreach (var userToBeRemoved in usersToBeRemoved)
        {
            _ = _accessManagementRepository.RemoveUser(userToBeRemoved.Email);
        }

        return fileProcessingResult;
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