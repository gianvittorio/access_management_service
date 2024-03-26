using System.Text.Json.Serialization;

namespace AccessManagementService.Web.Dtos.EmployerSignup;

public class EmployerSignupResponseDto
{
    [JsonPropertyName("processed_lines")]
    public IList<long> ProcessedLines { get; set; } = new List<long>();

    [JsonPropertyName("skipped_lines")]
    public IList<long> SkippedLines { get; set; } = new List<long>();
}