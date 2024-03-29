namespace AccessManagementService.Persistence.Entities;

public class EligibilityMetadataEntity
{
    public string? Id { get; set; }
    public string FileUrl { get; set; } = null!;
    public string EmployerName { get; set; } = null!;
}