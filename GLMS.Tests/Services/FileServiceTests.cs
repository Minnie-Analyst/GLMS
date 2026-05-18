using GLMS.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace GLMS.Tests.Services
{
    public class FileServiceTests
    {
        [Fact]
        public async Task SavePdfAsync_ReturnsNull_ForInvalidFileType()
        {
            // ARRANGE

            var service = new FileService();

            var file = new FormFile(
                Stream.Null,
                0,
                0,
                "Data",
                "image.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            // ACT

            var result = await service.SavePdfAsync(file);

            // ASSERT

            Assert.Null(result);
        }
    }
}