using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessManagementService.Persistence.Entities;

[Table("EligibilityMetadata")]
[Microsoft.EntityFrameworkCore.Index(nameof(EmployerName), IsUnique = true)]
public class EligibilityMetadataEntity
{
    [Key]
    public int EmployerId { get; set; }
    public string FileUrl { get; set; } = null!;
    public string EmployerName { get; set; } = null!;
}