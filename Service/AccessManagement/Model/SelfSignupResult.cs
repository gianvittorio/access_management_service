namespace AccessManagementService.Service.AccessManagement.Model;

public class SelfSignupResult
{
    public int? UserId { get; set; }
    public UserAccessType UserAccessType { get; set; }
}