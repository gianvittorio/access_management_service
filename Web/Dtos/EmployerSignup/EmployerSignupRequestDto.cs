using System.Text.Json.Serialization;

namespace AccessManagementService.Web.Dtos.EmployerSignup;

public class EmployerSignupRequestDto
{
    [JsonPropertyName("file")]
    public string FileUrl { get; set; }
    
    [JsonPropertyName("employer_name")]
    public string EmployerName { get; set; }
}