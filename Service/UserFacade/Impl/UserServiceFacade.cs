using System.Text;
using System.Text.Json;
using AccessManagementService.Service.UserFacade.Dtos;
using AutoFixture;

namespace AccessManagementService.Service.UserFacade.Impl;

public class UserServiceFacade : IUserServiceFacade
{
    private static readonly Fixture AutoFixture = new();
    private const string SaveUserEndpoint = "api/v1/users";
    
    private readonly string _userServiceUrl;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    
    public UserServiceFacade(IConfiguration configuration, HttpClient httpClient, JsonSerializerOptions serializerOptions)
    {
        _userServiceUrl = configuration["wiremock_host"]!;
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
    }
    
    public Task<UserResponseDto?> FindUserByUserIdAsync(string userId)
    {
        var userResponseDto = AutoFixture.Build<UserResponseDto>()
            .With(user => user.Id, userId)
            .Create();

        return Task.FromResult(userResponseDto)!;
    }

    public Task<UserResponseDto?> FindUserByEmailAsync(string email)
    {
        var userResponseDto = AutoFixture.Build<UserResponseDto>()
            .With(user => user.Email, email)
            .Create();
        
        return Task.FromResult(userResponseDto)!;
    }
    
    public async Task<UserResponseDto> SaveUserAsync(UserRequestDto userRequestDto)
    {
        var hostUri = new Uri(_userServiceUrl);
        var requestUri = new UriBuilder
        {
            Scheme = hostUri.Scheme,
            Host = hostUri.Host,
            Port = hostUri.Port,
            Path = SaveUserEndpoint
        }.Uri;

        var contentJsonString = JsonSerializer.Serialize(userRequestDto);
        var response = await _httpClient.PostAsync(requestUri, new StringContent(contentJsonString, Encoding.UTF8));

        var responseBodyString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UserResponseDto>(responseBodyString, _serializerOptions)!;
    }
    
    public Task RemoveUserAsync(string userEmail)
    {
        return Task.CompletedTask;
    }
}