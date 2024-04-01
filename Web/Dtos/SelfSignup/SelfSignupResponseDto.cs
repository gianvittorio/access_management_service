using System.Text.Json.Serialization;

namespace AccessManagementService.Web.Dtos.SelfSignup;

public class SelfSignupResponseDto
{
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("access_type")]
    public string UserAccessType { get; set; } = null!;
}