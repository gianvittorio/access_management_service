namespace AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;

public class FileProcessingResult
{
    public string EmployerName { get; set; } = null!;

    public IList<User> Users { get; set; } = new List<User>();
    
    public IList<string> ProcessedLines { get; set; } = new List<string>();

    public IList<string> SkippedLines { get; set; } = new List<string>();
}