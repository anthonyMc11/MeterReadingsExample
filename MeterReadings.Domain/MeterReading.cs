namespace MeterReadings.Domain
{
    public record MeterReading(int AccountId, DateTime MeterReadingDateTime, string MeterReadingValue);
}
