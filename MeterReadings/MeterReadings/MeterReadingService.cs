namespace MeterReadings.MeterReadings;

public record CsvImportResult(int Successful, int Failures);

public class MeterReadingService : IMeterReadingService
{
    public Task<CsvImportResult> ProcessCsvImport(IFormFile file)
    {
        return Task.FromResult(new CsvImportResult(0,0));
    }
}



public interface IMeterReadingService
{
    public Task<CsvImportResult> ProcessCsvImport(IFormFile file);
}