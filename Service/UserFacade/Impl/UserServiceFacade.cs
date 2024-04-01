using System.Text;
using System.Text.Json;
using AccessManagementService.Service.UserFacade.Dtos;
using AutoFixture;
using Microsoft.Extensions.Caching.Distributed;

namespace AccessManagementService.Service.UserFacade.Impl;

public class UserServiceFacade : IUserServiceFacade
{
    private static readonly Fixture AutoFixture = new();
    private const string SaveUserEndpoint = "api/v1/users";
    private readonly string _userServiceUrl;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly IDistributedCache _cache;
    
    public UserServiceFacade(IConfiguration configuration, HttpClient httpClient, JsonSerializerOptions serializerOptions, IDistributedCache cache)
    {
        _userServiceUrl = configuration["wiremock_host"]!;
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
        _cache = cache;
    }
    
    public async Task<UserResponseDto?> FindUserByEmailAsync(string email)
    {
        var userResponseDtoString = await _cache.GetStringAsync(email);
        if (string.IsNullOrWhiteSpace(userResponseDtoString))
        {
            var hostUri = new Uri(_userServiceUrl);
            var requestUri = new UriBuilder
            {
                Scheme = hostUri.Scheme,
                Host = hostUri.Host,
                Port = hostUri.Port,
                Path = SaveUserEndpoint,
                Query = $"email={email}"
            }.Uri;
            var response = await _httpClient.GetAsync(requestUri);
            userResponseDtoString = await response.Content.ReadAsStringAsync();
            await _cache.SetStringAsync(email, userResponseDtoString);
        }
        
        var userResponseDto = JsonSerializer.Deserialize<UserResponseDto>(userResponseDtoString, _serializerOptions);

        return userResponseDto;
    }

    public async Task<List<UserResponseDto>> FindUsersByEmployerIdAsync(string employerId)
    {
        var responseDtoString = await _cache.GetStringAsync(employerId);
        if (string.IsNullOrWhiteSpace(responseDtoString))
        {
            var hostUri = new Uri(_userServiceUrl);
            var requestUri = new UriBuilder
            {
                Scheme = hostUri.Scheme,
                Host = hostUri.Host,
                Port = hostUri.Port,
                Path = SaveUserEndpoint,
                Query = $"employer_id={employerId}"
            }.Uri;
            var response = await _httpClient.GetAsync(requestUri);
            responseDtoString = await response.Content.ReadAsStringAsync();
            await _cache.SetStringAsync(employerId, responseDtoString);
        }
        
        var userResponseDtos = JsonSerializer.Deserialize<List<UserResponseDto>>(responseDtoString, _serializerOptions);
        return userResponseDtos ?? new List<UserResponseDto>();
    }

    public async Task<UserResponseDto> SaveUserAsync(UserRequestDto userRequestDto)
    {
        var userRequestDtoJsonString = JsonSerializer.Serialize(userRequestDto, _serializerOptions);
        await _cache.SetStringAsync(userRequestDto.Email, userRequestDtoJsonString);
        
        var hostUri = new Uri(_userServiceUrl);
        var requestUri = new UriBuilder
        {
            Scheme = hostUri.Scheme,
            Host = hostUri.Host,
            Port = hostUri.Port,
            Path = SaveUserEndpoint
        }.Uri;

        var response = await _httpClient.PostAsync(requestUri, new StringContent(userRequestDtoJsonString, Encoding.UTF8));
        var userResponseDtoJsonString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UserResponseDto>(userResponseDtoJsonString, _serializerOptions)!;
    }
    
    public Task RemoveUserAsync(string userEmail)
    {
        return Task.CompletedTask;
    }
}