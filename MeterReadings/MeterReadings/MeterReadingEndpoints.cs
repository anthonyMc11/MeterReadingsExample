using MeterReadings.Contracts;

namespace MeterReadings.MeterReadings;

public static class MeterReadingEndpoints
{
    public static void MapMeterReadingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("meter-readings");

        group.MapPost("/meter-reading-uploads", async (IMeterReadingService meterReadingService, IFormFile csvFile ) => { 
            if(csvFile is null)
            {
                return Results.BadRequest();
            }
            else
            {
                var result = await meterReadingService.ProcessCsvImport(csvFile);
                return Results.Ok(result.MapToUploadResponse());
            }
        })
        .DisableAntiforgery();
    }
}
