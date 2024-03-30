using System.Text.Json.Serialization;
using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Web.Dtos.SelfSignup;

public class SelfSignupResponseDto
{
    [JsonPropertyName("user_id")]
    public int? UserId { get; set; }

    [JsonPropertyName("access_type")]
    public string UserAccessType { get; set; } = null!;
}