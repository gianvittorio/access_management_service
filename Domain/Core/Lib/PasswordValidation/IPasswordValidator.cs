namespace AccessManagementService.Domain.Core.Lib.PasswordValidation;

public interface IPasswordValidator
{
    public class PasswordValidationResult
    {
        public bool IsValid { get; set; }

        public bool HasMinNumberOfCharacters { get; set; }

        public bool HasLetter { get; set; }

        public bool HasDigit { get; set; }

        public bool HasSymbol { get; set; }
    }
    
    PasswordValidationResult Validate(string password);
}