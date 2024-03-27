namespace AccessManagementService.Service.AccessManagement.Model;

public class SelfSignupResult
{
    public string UserId { get; set; } = null!;
    public UserAccessType UserAccessType { get; set; }
}