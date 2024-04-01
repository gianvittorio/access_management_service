using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AccessManagementService.Service.UserFacade.Dtos;
using AutoFixture;

namespace AccessManagementService.Service.UserFacade.Impl;

public class UserServiceFacade : IUserServiceFacade
{
    private static readonly Fixture AutoFixture = new();

    private readonly HttpClient _httpClient;

    public UserServiceFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

        //return Task.FromResult<UserResponseDto?>(null);
    }
    
    public async Task<UserResponseDto> SaveUserAsync(UserRequestDto userRequestDto)
    {
        var requestUri = new UriBuilder
        {
            Host = "localhost",
            Scheme = "http",
            Port = 8083,
            Path = "api/v1/users"
        }.Uri;

        var contentJsonString = JsonSerializer.Serialize(userRequestDto);
        var response = await _httpClient.PostAsync(requestUri, new StringContent(contentJsonString, Encoding.UTF8));

        var responseBodyString = await response.Content.ReadAsStringAsync();

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
        return JsonSerializer.Deserialize<UserResponseDto>(responseBodyString, serializerOptions)!;
    }
    
    public Task RemoveUserAsync(string userEmail)
    {
        return Task.CompletedTask;
    }
}