using CsvHelper;
using CsvHelper.Configuration;

namespace MeterReadings.MeterReadings;

public record CsvImportResult(int Successful, int Failures);

public class MeterReadingService(IMeterReadingValidator validator) : IMeterReadingService
{
    public CsvImportResult ProcessCsvImport(IFormFile file)
    {
        var records = GetRecords(file);
        return ProcessRecords(records);
    }

    private static FrozenSet<MeterReadingUploadRequest> GetRecords(IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        ms.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(ms);
        using var csv = new CsvReader(reader, GetCsvConfiguration());
       
        csv.Context.RegisterClassMap<MeterReadingUploadRequestMap>();
        try
        {
            return csv.GetRecords<MeterReadingUploadRequest>().ToFrozenSet();
        }
        catch(HeaderValidationException)
        {
         //  an invalid file was uploaded
        }
        return FrozenSet<MeterReadingUploadRequest>.Empty;
    }

    private CsvImportResult ProcessRecords(FrozenSet<MeterReadingUploadRequest> records)
    {
        int successful = 0;
        int failure = 0;

       foreach(var record in records)
        {
            var validationResult = validator.Validate(record);

            if (validationResult.IsValid)
            {
                successful++;
            }
            else
            {
                failure++;
            } 
        }

        return new CsvImportResult(successful, failure);
    }


    private static CsvConfiguration GetCsvConfiguration(){
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            BadDataFound = null, // protects against invalid file uploads
            ReadingExceptionOccurred = re =>
            {
                return false;
            }
        };
        return config;
    }
    private class MeterReadingUploadRequestMap : ClassMap<MeterReadingUploadRequest>
    {
        public MeterReadingUploadRequestMap()
        {
            Map(m => m.AccountId).Name("AccountId");
            Map(m => m.MeterReadingDateTime).Name("MeterReadingDateTime")
                .TypeConverter<CsvHelper.TypeConversion.DateTimeConverter>()
            .TypeConverterOption.Format("dd/MM/yyyy HH:mm"); ;
            Map(m => m.MeterReadValue).Name("MeterReadValue");
        }
    }
}

public interface IMeterReadingService
{
    public CsvImportResult ProcessCsvImport(IFormFile file);
}