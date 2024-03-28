using System.Globalization;
using AccessManagementService.Domain.Core.Lib.EligibilityFileProcessing;
using AccessManagementService.Service.AccessManagement.Model;
using CsvHelper;
using CsvHelper.Configuration;

namespace AccessManagementService.Domain.Core.Lib.CsvFileProcessing.Impl;

public class EligibilityFileStreamProcessor(string employerName) : IEligibilityFileStreamProcessor
{
    private readonly CsvConfiguration _csvConfiguration = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        Delimiter = ","
    };
    
    public async Task<FileProcessingResult> Process(StreamReader csvStreamReader)
    {
        var fileProcessingResult = new FileProcessingResult { EmployerName = employerName };

        using var csvReader = new CsvReader(csvStreamReader, _csvConfiguration);
        csvReader.Context.RegisterClassMap<UserMap>();
        await csvReader.ReadAsync();
        csvReader.ReadHeader();
        while (await csvReader.ReadAsync())
        {
            try
            {
                fileProcessingResult.Users.Add(csvReader.GetRecord<User>());
                fileProcessingResult.ProcessedLines.Add(csvReader.Parser.RawRecord);
            }
            catch (Exception)
            {
                fileProcessingResult.SkippedLines.Add(csvReader.Parser.RawRecord);
            }
        }

        return fileProcessingResult;
    }
}