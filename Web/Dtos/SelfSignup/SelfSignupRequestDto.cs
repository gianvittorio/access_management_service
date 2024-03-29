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

    [JsonPropertyName("full_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public String? FullName { get; set; }

    [JsonPropertyName("country")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Country { get; set; }

    [JsonPropertyName("birth_date")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime? BirthDate { get; set; }

    [JsonPropertyName("salary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Decimal? Salary { get; set; }

    [JsonPropertyName("employer_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EmployerName { get; set; }
}