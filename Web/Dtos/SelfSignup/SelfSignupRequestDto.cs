using System.Text.Json.Serialization;

namespace AccessManagementService.Web.Dtos.SelfSignup;

public class SelfSignupRequestDto
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
}