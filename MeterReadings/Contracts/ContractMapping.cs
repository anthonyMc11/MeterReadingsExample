namespace MeterReadings.Contracts;

[ExcludeFromCodeCoverage]
public static class ContractMapping
{
    public static MeterReadingCsvUploadResponse MapToUploadResponse(this CsvImportResult result) 
        => new(result.Successful, result.Failures);
}