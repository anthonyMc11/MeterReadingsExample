using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Domain;
public class Account{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    }
