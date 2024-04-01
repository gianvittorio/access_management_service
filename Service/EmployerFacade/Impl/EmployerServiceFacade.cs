using System.Text.Json;
using AccessManagementService.Service.EmployerFacade.Dtos;
using Microsoft.Extensions.Caching.Distributed;

namespace AccessManagementService.Service.EmployerFacade.Impl;

public class EmployerServiceFacade : IEmployerServiceFacade
{
    private const string FindEmployerByNameEndpoint = "api/v1/employers";
    private readonly string _userServiceUrl;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private IDistributedCache _cache;

    public EmployerServiceFacade(IConfiguration configuration, HttpClient httpClient, JsonSerializerOptions serializerOptions, IDistributedCache cache)
    {
        _userServiceUrl = configuration["wiremock_host"]!;
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
        _cache = cache;
    }

    public async Task<string> FindEmployerIdByEmployerName(string employerName)
    {
        var responseDtoJsonString = await _cache.GetStringAsync(employerName);
        if (string.IsNullOrWhiteSpace(responseDtoJsonString))
        {
            var hostUri = new Uri(_userServiceUrl);
            var query = $"name={employerName}";
            var requestUri = new UriBuilder
            {
                Scheme = hostUri.Scheme,
                Host = hostUri.Host,
                Port = hostUri.Port,
                Path = FindEmployerByNameEndpoint,
                Query = query
            }.Uri;
            var response = await _httpClient.GetAsync(requestUri);
            responseDtoJsonString = await response.Content.ReadAsStringAsync();
            await _cache.SetStringAsync(employerName, responseDtoJsonString);
        }
        
        var responseDto = JsonSerializer.Deserialize<EmployerResponseDto>(responseDtoJsonString, _serializerOptions)!;
        return responseDto.Id;
    }
}