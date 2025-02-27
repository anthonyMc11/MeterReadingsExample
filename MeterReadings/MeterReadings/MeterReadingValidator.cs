using FluentValidation.Results;
using System.Text.RegularExpressions;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace MeterReadings.MeterReadings;

//development notes:
//(DB select: Account with left join on top 1 meter reading ordered by date)
//If the account does not exist return error
//If previous meter reading then:
//if meter reading date is less than last return error
//If the raw value is not exactly 5 numeric digits return error
//If the value is less than the previous reading, return error

public partial class MeterReadingValidator(IRepository<Account> accountRepository) : IMeterReadingValidator
{
    private const string MeterReadingFieldName = nameof(MeterReadingUploadRequest.MeterReadValue);
    private const string AccountIdFieldName = nameof(MeterReadingUploadRequest.AccountId);

    [GeneratedRegex("^[0-9]{5}$")]
    public static partial Regex MeterReadingValue();
    private readonly Regex MeterValueRegex = MeterReadingValue();

    public ValidationResult Validate(MeterReadingUploadRequest uploadedMeterReading)
    {
        var validationFailures = new List<ValidationFailure>();

        validationFailures.AddRange(ValidateAccountId(uploadedMeterReading.AccountId).Errors);
        validationFailures.AddRange(ValidateMeterReading(uploadedMeterReading.MeterReadValue).Errors);

        return new ValidationResult(validationFailures);
    }

   private ValidationResult ValidateAccountId(int accountId)
    {
        var validationFailures = new List<ValidationFailure>();
        Account? account = accountRepository.GetById(accountId);

        if (account is null)
        {
            validationFailures.Add(new ValidationFailure(AccountIdFieldName, "AccountId does not exist"));
        }

        return new ValidationResult(validationFailures);
    }

    private ValidationResult ValidateMeterReading(string meterReadingValue)
    {
        var result = new ValidationResult();

        if (!MeterValueRegex.IsMatch(meterReadingValue))
        {
            result.Errors.Add(new(MeterReadingFieldName, "The meter reading is not on the format NNNNN"));
            return result;
        }
        return result;
    }
}

public interface IMeterReadingValidator
{
    public ValidationResult Validate(MeterReadingUploadRequest uploadedMeterReading);
}