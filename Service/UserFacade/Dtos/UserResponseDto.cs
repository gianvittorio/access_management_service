using System.Text.Json.Serialization;

namespace AccessManagementService.Service.UserFacade.Dtos;

public class UserResponseDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
    
    [JsonPropertyName("country")]
    public string Country { get; set; } = null!;
    
    [JsonPropertyName("access_type")]
    public string AccessType { get; set; } = null!;
    
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }
    
    [JsonPropertyName("employer_id")]
    public string? EmployerId { get; set; }
    
    [JsonPropertyName("birth_date")]
    public DateTime? BirthDate { get; set; }
    
    [JsonPropertyName("salary")]
    public Decimal? Salary { get; set; }
}