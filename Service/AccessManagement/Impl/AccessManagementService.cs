using System.Globalization;
using System.Net;
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
using CsvHelper;
using CsvHelper.Configuration;

namespace AccessManagementService.Service.AccessManagement.Impl;

public class AccessManagementService : IAccessManagementService
{
    private static readonly Fixture AutoFixture = new();
    private HttpClient _httpClient;
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
                BirthDate = DateTime.Parse(employeeUser.BirthDate, CultureInfo.InvariantCulture),
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

    public async Task<FileProcessingResult> DownloadAndProcessEligibilityFileAsync(string fileUrl, string employerName)
    {
        using var response = await _httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
        await using var responseContentStream = await response.Content.ReadAsStreamAsync();
        using var streamReader = new StreamReader(responseContentStream);
        var eligibilityFileStreamProcessor = new EligibilityFileStreamProcessor(employerName);
        var fileProcessingResult = await eligibilityFileStreamProcessor.Process(streamReader);

        var usersSaveTasks = fileProcessingResult.Users.Select(user => new UserEntity
            {
                Email = user.Email,
                FullName = user.FullName,
                Country = user.Country,
                BirthDate = DateTime.Parse(user.BirthDate, DateTimeFormatInfo.InvariantInfo),
                Salary = user.Salary,
                EmployerName = employerName
            })
            .Select(userEntity => _accessManagementRepository.SaveUser(userEntity))
            .Cast<Task>()
            .ToList();
        await Task.WhenAll(usersSaveTasks);

        var usersForEmployerName = (await _accessManagementRepository.FindUsersByEmployerName(employerName));
        var usersToBeRemoved = usersForEmployerName
            .ExceptBy(fileProcessingResult.Users.Select(user => user.Email), user => user.Email);
        
        var usersRemoveTasks = usersToBeRemoved
            .Select(userEntity => _accessManagementRepository.RemoveUser(userEntity.Email)).ToList();
        await Task.WhenAll(usersRemoveTasks);

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