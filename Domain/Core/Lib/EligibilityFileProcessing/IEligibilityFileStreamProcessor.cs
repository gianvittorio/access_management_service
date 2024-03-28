using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
using AccessManagementService.Service.AccessManagement.Model;

namespace AccessManagementService.Domain.Core.Lib.CsvFileProcessing;

public interface IEligibilityFileStreamProcessor
{
    Task<FileProcessingResult> Process(StreamReader csvStreamReader);
}