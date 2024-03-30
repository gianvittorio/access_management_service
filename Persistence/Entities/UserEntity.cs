using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AccessManagementService.Persistence.Entities;

[Table("Users")]
[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? FullName { get; set; }
    public string? Country { get; set; }
    public DateTime? BirthDate { get; set; }
    public Decimal? Salary { get; set; }
    public int? EmployerId { get; set; }

    public EligibilityMetadataEntity EligibilityMetadataEntity { get; set; } = null!;
}