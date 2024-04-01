namespace AccessManagementService.Domain.Core.Lib.PasswordValidation.Impl;

public class PasswordValidator : IPasswordValidator
{
    public bool Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        
        if (password.Length < 8)
        {
            return false;
        }

        bool hasLetter = false;
        bool hasDigit = false;
        bool hasSymbol = false;
        for (int i = 0; i < password.Length; i++)
        {
            var character = password[i];
            if (char.IsLetter(character))
            {
                hasLetter = true;
            }
            
            if (char.IsDigit(character))
            {
                hasDigit = true;
            }

            if (char.IsSymbol(character))
            {
                hasSymbol = true;
            }
            
            if (hasLetter && hasDigit && hasSymbol)
            {
                return true;
            }
        }

        return false;
    }
}