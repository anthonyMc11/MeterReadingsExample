namespace MeterReadings.MeterReadings;

public static  class MeterReadingEndpoints
{

    public static void MapMeterReadingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("meter-readings");

        group.MapPost("/meter-reading-uploads", async (IMeterReadingService meterReadingService, IFormFile file ) => { 
            if(file is null)
            {
                return Results.BadRequest();
            }
            else
            {
                return Results.Ok();
            }
        })
        .DisableAntiforgery() ;
    }
}
