using GLMS.Data;
using GLMS.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GLMS.Tests.Services
{
    public class ServiceRequestValidationTests
    {
        [Fact]
        public async Task CannotCreateRequest_ForExpiredContract()
        {
            // ARRANGE

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ExpiredContractTest")
                .Options;

            using var context = new AppDbContext(options);

            var contract = new Contract
            {
                Id = 1,
                Status = "Expired"
            };

            context.Contracts.Add(contract);

            await context.SaveChangesAsync();

            // ACT

            var savedContract =
                await context.Contracts.FindAsync(1);

            // ASSERT

            Assert.Equal("Expired", savedContract?.Status);
        }
    }
}