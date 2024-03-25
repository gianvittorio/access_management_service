namespace AccessManagementService.Service.AccessManagement.Model;

public enum AccessType
{
    Dtc, Employer
}

public class User
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public AccessType AccessType { get; set; }
    public string? FullName { get; set; }
    public string? UserId { get; set; }
    public string? EmployerId { get; set; }
    public DateTime? BirthDate { get; set; }
    public Decimal? Salary { get; set; }
}