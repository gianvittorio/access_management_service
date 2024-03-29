namespace AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;

public class UserCredentials : User
{
    public string Password { get; set; } = null!;

    public string? EmployerName { get; set; }
}