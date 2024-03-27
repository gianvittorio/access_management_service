namespace AccessManagementService.Service.AccessManagement.Model;

public class User
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string BirthDate { get; set; }
    public Decimal Salary { get; set; }
}