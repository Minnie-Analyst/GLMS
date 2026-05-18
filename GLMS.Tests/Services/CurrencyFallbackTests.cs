using GLMS.Services;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace GLMS.Tests.Services
{
    public class CurrencyFallbackTests
    {
        [Fact]
        public async Task GetUsdToZarRate_ReturnsFallbackRate_WhenApiFails()
        {
            // ARRANGE

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ThrowsAsync(new HttpRequestException());

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new CurrencyService(httpClient);

            // ACT

            var rate = await service.GetUsdToZarRate();

            // ASSERT

            Assert.Equal(18.50m, rate);
        }
    }
}