namespace AccessManagementService.Service.AccessManagement.Model;

public class SelfSignupResult
{
    public bool SignedIn { get; set; }
    public string? UserId { get; set; }
    public UserAccessType UserAccessType { get; set; }
}