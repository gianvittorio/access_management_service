using System.Text.Json.Serialization;

namespace AccessManagementService.Web.Dtos.EmployerSignup;

public class EmployerSignupResponseDto
{
    [JsonPropertyName("processed_lines")]
    public IList<string> ProcessedLines { get; set; } = new List<string>();

    [JsonPropertyName("skipped_lines")]
    public IList<string> SkippedLines { get; set; } = new List<string>();
}