namespace MeterReadings.Contracts.Requests;

public class MeterReadingUploadRequest()
{
    public int AccountId { get; set; } 
    public DateTime MeterReadingDateTime { get; set; }
    public string MeterReadValue { get; set; } = "";
}