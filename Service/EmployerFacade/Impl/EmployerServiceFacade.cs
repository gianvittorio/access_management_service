using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccessManagementService.Service.EmployerFacade.Impl;

public class EmployerServiceFacade : IEmployerServiceFacade
{
    private const string FindEmployerByNameEndpoint = "api/v1/employers";
    
    private readonly string _userServiceUrl;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public EmployerServiceFacade(IConfiguration configuration, HttpClient httpClient, JsonSerializerOptions serializerOptions)
    {
        _userServiceUrl = configuration["wiremock_host"]!;
        _httpClient = httpClient;
        _serializerOptions = serializerOptions;
    }

    public async Task<string?> FindEmployerIdByEmployerName(string employerName)
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
        
        var responseBodyString = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<string>(responseBodyString, _serializerOptions)!;
    }
}