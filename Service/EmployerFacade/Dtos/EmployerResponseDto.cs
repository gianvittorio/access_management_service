using System.Text.Json.Serialization;

namespace AccessManagementService.Service.EmployerFacade.Dtos;

public class EmployerResponseDto
{
    [JsonPropertyName("id")]
    [JsonRequired]
    public string Id { get; set; } = null!;
}