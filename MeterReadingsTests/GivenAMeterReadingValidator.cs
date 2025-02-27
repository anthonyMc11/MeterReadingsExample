using MeterReadings.Domain;
using MeterReadings.MeterReadings;
using MeterReadings.Repositories;
using MeterReadings.Contracts.Requests;
using Microsoft.IdentityModel.Tokens;

namespace MeterReadingsTests;

public class GivenAMeterReadingValidator
{
    private readonly MeterReadingValidator _sut;
    //TODO: this list implementation would move to the test project once the database version is completed
    private readonly IRepository<Account> _accountRepository = new AccountsListRepository();

    private readonly MeterReadingUploadRequest _invalidUserIdRequest = new(){ AccountId = 100000, MeterReadingDateTime = DateTime.Now, MeterReadValue = "12345" };
    private readonly MeterReadingUploadRequest validMeterReading = new() { AccountId = 1, MeterReadingDateTime = DateTime.Now, MeterReadValue = "12345" };

    public GivenAMeterReadingValidator()
    {
        SeedDatabase(_accountRepository);
        _sut = new MeterReadingValidator(_accountRepository);
    }

    private static void SeedDatabase(IRepository<Account> accountRepository)
    {
        accountRepository.Add(new( 1, "bob", "test" ));
        accountRepository.Add(new( 2, "bob", "test" ));
        accountRepository.Add(new( 3, "bob", "test" ));
        accountRepository.Add(new( 4, "bob", "test" ));
        accountRepository.Add(new( 5, "bob", "test" ));
        accountRepository.Add(new( 6, "bob", "test" ));
    }

    [Fact]
    public void WhenAValidMeterReading_ThenNoValidationErrors()
    {
        var result = _sut.Validate(validMeterReading);

        Assert.True(result.IsValid);
        Assert.True(result.Errors.IsNullOrEmpty());
    }

    [Fact]
    public void WhenAnUnknownAccountIsEncountered_ThenValidationErrorIsRaise()
    {
        var result = _sut.Validate(_invalidUserIdRequest);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Exists(x => x.PropertyName == nameof(MeterReadingUploadRequest.AccountId)));
    }

    [Theory]
    [InlineData("1")]
    [InlineData("-6575")]
    [InlineData("323")]
    [InlineData("999999")]
    [InlineData("0")]
    [InlineData("676")]
    public void WhenAnInvalidMeterReadingIsEncountered_ThenValidationErrorIsRaise(string invalidMeterReadingValue)
    {
        MeterReadingUploadRequest invalidMeterReading = new() { AccountId = 1, MeterReadingDateTime = DateTime.Now, MeterReadValue = invalidMeterReadingValue };

        var result = _sut.Validate(invalidMeterReading);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Exists(x => x.PropertyName == nameof(MeterReadingUploadRequest.MeterReadValue)));
    }
}
