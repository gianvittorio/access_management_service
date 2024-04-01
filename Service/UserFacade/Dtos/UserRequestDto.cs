using System.Text.Json.Serialization;
using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Service.UserFacade.Dtos;

public class UserRequestDto
{
    [JsonPropertyName("email")]
    [JsonRequired]
    public string Email { get; set; } = null!;
    
    [JsonPropertyName("password")]
    [JsonRequired]
    public string Password { get; set; } = null!;
    
    [JsonPropertyName("country")]
    [JsonRequired]
    public string Country { get; set; } = null!;
    
    [JsonPropertyName("access_type")]
    [JsonRequired]
    public string AccessType { get; set; } = null!;
    
    [JsonPropertyName("full_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? FullName { get; set; }
    
    [JsonPropertyName("employer_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EmployerId { get; set; }
    
    [JsonPropertyName("birth_date")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime? BirthDate { get; set; }
    
    [JsonPropertyName("salary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Decimal? Salary { get; set; }
}