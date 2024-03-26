namespace AccessManagementService.Service.AccessManagement.Model;

public class FileProcessingResult
{
    public IList<long> ProcessedLines { get; set; } = new List<long>();

    public IList<long> SkippedLines { get; set; } = new List<long>();
}