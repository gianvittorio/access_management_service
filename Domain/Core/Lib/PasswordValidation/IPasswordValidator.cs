namespace AccessManagementService.Domain.Core.Lib.PasswordValidation;

public interface IPasswordValidator
{
    bool Validate(string password);
}