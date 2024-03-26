using System.Text.Json.Serialization;

namespace AccessManagementService.Web.Dtos.SelfSignup;

public class SelfSignupResponseDto
{
    [JsonPropertyName("signed_in")]
    public bool SignedIn { get; set; }

    [JsonPropertyName("user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? UserId { get; set; }
    
    [JsonPropertyName("employer_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EmployerId { get; set; }
}