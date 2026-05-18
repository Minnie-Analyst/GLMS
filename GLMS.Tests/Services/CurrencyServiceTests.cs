using GLMS.Services;
using Xunit;

namespace GLMS.Tests.Services
{
    public class CurrencyServiceTests
    {
        [Fact]
        public async Task GetUsdToZarRate_ReturnsValidRate()
        {
            // ARRANGE
            var httpClient = new HttpClient();

            var service = new CurrencyService(httpClient);

            // ACT
            var rate = await service.GetUsdToZarRate();

            // ASSERT
            Assert.True(rate > 0);
        }
    }
}