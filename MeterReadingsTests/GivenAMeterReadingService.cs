using MeterReadings.MeterReadings;
using Microsoft.AspNetCore.Http;
using Moq;

namespace MeterReadingsTests
{
    public class GivenAMeterReadingService
    {
        public IMeterReadingService _sut;
        public Mock<IMeterReadingValidator> _validator = new();

        public GivenAMeterReadingService()
        {
        
            _sut = new MeterReadingService(_validator.Object);
        }
        [Fact]
        public void WhenAFileIsUploaded_ThenAResultIsReturned()
        {
            Mock<IFormFile> mockFormFile = new();
            var result = _sut.ProcessCsvImport(mockFormFile.Object);
            Assert.NotNull(result);
        }
    }
}