namespace AccessManagementService.Domain.Core.Lib.PasswordValidation.Impl;

public class PasswordValidator : IPasswordValidator
{
    public IPasswordValidator.PasswordValidationResult Validate(string password)
    {
        var passwordValidationResult = new IPasswordValidator.PasswordValidationResult();
        if (string.IsNullOrWhiteSpace(password))
        {
            return passwordValidationResult;
        }

        if (password.Length < 8)
        {
            return passwordValidationResult;
        }
        
        passwordValidationResult.HasMinNumberOfCharacters = true;

        foreach (var character in password)
        {
            if (char.IsLetter(character))
            {
                passwordValidationResult.HasLetter = true;
            }
            
            if (char.IsDigit(character))
            {
                passwordValidationResult.HasDigit = true;
            }
            
            if (char.IsSymbol(character))
            {
                passwordValidationResult.HasSymbol = true;
            }
            
            passwordValidationResult.IsValid = passwordValidationResult is
            {
                HasMinNumberOfCharacters: true, HasLetter: true, HasDigit: true, HasSymbol: true
            };
            if (passwordValidationResult.IsValid)
            {
                break;
            }
        }

        return passwordValidationResult;
    }
}