namespace MeterReadings.Contracts.Requests;

public record MeterReadingUploadRequest(int AccountId, DateTime MeterReadingDateTime, string MeterReadValue);

