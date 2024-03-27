using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Service.UserFacade.Dtos;

public class UserRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public UserAccessType AccessType { get; set; }
    public string? FullName { get; set; }
    public string? EmployerId { get; set; }
    public DateTime? BirthDate { get; set; }
    public Decimal? Salary { get; set; }
}