using MeterReadings.Repositories;

namespace MeterReadings.MeterReadings;

//development notes:
//(DB select: Account with left join on top 1 meter reading ordered by date)
//If the account does not exist return error
//If previous meter reading then:
//if meter reading date is less than last return error
//If the raw value is not exactly 5 numeric digits return error
//If the value is less than the previous reading, return error

public class MeterReadingValidator(IRepository<Account> accountRepository) : IMeterReadingValidator
{
    public ValidationResult Validate(MeterReadingUploadRequest uploadedMeterReading)
    { 
        var validationFailures = new List<ValidationFailure>();

        Account? account = accountRepository.GetById(uploadedMeterReading.AccountId);

        if(account is null)
        {
            validationFailures.Add(new ValidationFailure(nameof(uploadedMeterReading.AccountId), "AccountId does not exist"));
        }

        return new ValidationResult(validationFailures);
    }
}

public interface IMeterReadingValidator
{
    public ValidationResult Validate(MeterReadingUploadRequest uploadedMeterReading);
}