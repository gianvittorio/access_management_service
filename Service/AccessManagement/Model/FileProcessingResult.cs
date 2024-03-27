namespace AccessManagementService.Service.AccessManagement.Model;

public class FileProcessingResult
{
    public string EmployerName { get; set; } = null!;
    
    public IList<string> ProcessedLines { get; set; } = new List<string>();

    public IList<string> SkippedLines { get; set; } = new List<string>();
}