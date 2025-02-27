namespace MeterReadings.Domain;

public record MeterReading(int AccountId, DateTime MeterReadingDateTime, int MeterReadingValue);