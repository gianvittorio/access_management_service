using System.Globalization;
using System.Text.Json;
using AccessManagementService.Domain.Core.Lib.CsvFileProcessing.Impl;
using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
using AccessManagementService.Domain.Core.Lib.PasswordValidation;
using AccessManagementService.Domain.Core.Lib.PasswordValidation.Impl;
using AccessManagementService.Persistence.Entities;
using AccessManagementService.Persistence.Repository;
using AccessManagementService.Service.AccessManagement.Exceptions.PasswordValidation;
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
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly IPasswordValidator _passwordValidator = new PasswordValidator();
    

    public AccessManagementService(
        IUserServiceFacade userServiceFacade, 
        IAccessManagementRepository accessManagementRepository, 
        IEmployerServiceFacade employerServiceFacade, 
        HttpClient httpClient,
        JsonSerializerOptions serializerOptions
        )
    {
        _userServiceFacade = userServiceFacade;
        _accessManagementRepository = accessManagementRepository;
        _employerServiceFacade = employerServiceFacade;
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
    }

    public async Task<SelfSignupResult> SelfSignUpAsync(UserCredentials userCredentials)
    {
        var passwordValidationResult = _passwordValidator.Validate(userCredentials.Password);
        if (!passwordValidationResult.IsValid)
        {
            throw new PasswordValidationException { PasswordValidationResult = passwordValidationResult };
        }

        User? employeeUser = null;
        if (!string.IsNullOrWhiteSpace(userCredentials.EmployerName))
        {
            var eligibilityMetadataForEmployerName = await _accessManagementRepository.FindEligibilityMetadataEntityByEmployerNameAsync(userCredentials.EmployerName);
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

        var userResponseDto = await _userServiceFacade.FindUserByEmailAsync(userCredentials.Email);
        if (userResponseDto is null)
        {
            var userRequestDto = new UserRequestDto
            {
                Email = userCredentials.Email,
                Password = userCredentials.Password,
                Country = userCredentials.Country,
                AccessType = userAccessType.ToString(),
                FullName = userCredentials.FullName,
                BirthDate = DateTime.Parse(userCredentials.BirthDate, DateTimeFormatInfo.CurrentInfo).ToUniversalTime(),
                EmployerId = employerId
            };
            userResponseDto = await _userServiceFacade.SaveUserAsync(userRequestDto);
        }

        var selfSignupResult = new SelfSignupResult
        {
            UserId = userResponseDto.Id,
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
        
        var usersForEmployerId = await _userServiceFacade.FindUsersByEmployerIdAsync(persistedEligibilityMetadataEntity.EmployerId.ToString());
        var usersToBeRemoved = usersForEmployerId
            .ExceptBy(fileProcessingResult.Users.Select(user => user.Email), user => user.Email);
        foreach (var userToBeRemoved in usersToBeRemoved)
        {
            _ = await _accessManagementRepository.RemoveUserAsync(userToBeRemoved.Email);
        }
        
        foreach (var currentUser in fileProcessingResult.Users)
        {
            var currentUserResponseDto = await _userServiceFacade.FindUserByEmailAsync(currentUser.Email);
            if (currentUserResponseDto is null)
            {
                continue;
            }

            var userRequestDto = new UserRequestDto
            {
                Email = currentUserResponseDto.Email,
                Password = currentUserResponseDto.Password,
                Country = currentUserResponseDto.Country,
                AccessType = currentUserResponseDto.AccessType,
                FullName = currentUserResponseDto.FullName,
                BirthDate = currentUserResponseDto.BirthDate,
                EmployerId = currentUserResponseDto.EmployerId,
                Salary = currentUser.Salary
            };
            _ = await _userServiceFacade.SaveUserAsync(userRequestDto);
        }

        return fileProcessingResult;
    }

    private User? FindRegisteredUserByEmail(string email, FileProcessingResult fileProcessingResult)
    {
        return AutoFixture.Build<User>()
            .With(user => user.Email, email)
            .Create();
    }
}