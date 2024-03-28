namespace AccessManagementService.Persistence.Entities;

public class UserEntity
{
    public string? Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public Decimal Salary { get; set; }
    public string EmployerName { get; set; } = null!;
}