using MeterReadings.Contracts;

namespace MeterReadings.MeterReadings;

public static class MeterReadingEndpoints
{
    public static void MapMeterReadingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("meter-readings");

        group.MapPost("/meter-reading-uploads", static (IMeterReadingService meterReadingService, IFormFile csvFile ) => {
            var result = meterReadingService.ProcessCsvImport(csvFile);
            return Results.Ok(result.MapToUploadResponse());
        })
        .DisableAntiforgery();
    }
}