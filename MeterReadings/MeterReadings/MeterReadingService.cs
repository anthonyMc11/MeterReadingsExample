﻿using CsvHelper;
using CsvHelper.Configuration;

namespace MeterReadings.MeterReadings;

public record CsvImportResult(int Successful, int Failures);

public class MeterReadingService : IMeterReadingService
{
    public Task<CsvImportResult> ProcessCsvImport(IFormFile file)
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
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<MeterReadingUploadRequestMap>();
        return csv.GetRecords<MeterReadingUploadRequest>().ToFrozenSet();

    }

    private async Task<CsvImportResult> ProcessRecords(FrozenSet<MeterReadingUploadRequest> records)
    {
        int successful = 0;
        int failure = 0;

       foreach(var record in records)
        {
            successful++;

        }

        return new CsvImportResult(successful, failure);
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
    public Task<CsvImportResult> ProcessCsvImport(IFormFile file);
}