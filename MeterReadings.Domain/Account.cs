using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Domain;
public class Account{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";

    public Account()
    {

    }

    public Account(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
    }
