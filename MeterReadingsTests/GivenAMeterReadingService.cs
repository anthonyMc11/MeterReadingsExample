using MeterReadings.MeterReadings;
using Microsoft.AspNetCore.Http;
using Moq;

namespace MeterReadingsTests
{
    public class GivenAMeterReadingService
    {
        public IMeterReadingService _sut;

        public GivenAMeterReadingService()
        {
            _sut = new MeterReadingService();
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