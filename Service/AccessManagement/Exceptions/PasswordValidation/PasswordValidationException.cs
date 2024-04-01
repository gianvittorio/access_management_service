using AccessManagementService.Domain.Core.Lib.PasswordValidation;

namespace AccessManagementService.Service.AccessManagement.Exceptions.PasswordValidation;

public class PasswordValidationException : Exception
{
    public IPasswordValidator.PasswordValidationResult PasswordValidationResult { get; set; } = null!;
}