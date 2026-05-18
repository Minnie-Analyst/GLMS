using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GLMS.Tests.Services
{
    public class ContractFilterTests
    {
        [Fact]
        public async Task FilterContractsAsync_ReturnsOnlyActiveContracts()
        {
            // ARRANGE

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "FilterContractsTest")
                .Options;

            using var context = new AppDbContext(options);

            // ADD TEST CLIENT
            var client = new Client
            {
                Id = 1,
                Name = "John Doe",
                IdNumber = "1234567890123",
                ContactDetails = "john@test.com",
                PhoneNumber = "0712345678",
                Region = "Johannesburg"
            };

            context.Clients.Add(client);

            // ACTIVE CONTRACT
            context.Contracts.Add(new Contract
            {
                Id = 1,
                ClientId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Status = "Active",
                ServiceLevel = "Premium"
            });

            // EXPIRED CONTRACT
            context.Contracts.Add(new Contract
            {
                Id = 2,
                ClientId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Status = "Expired",
                ServiceLevel = "Standard"
            });

            await context.SaveChangesAsync();

            var service = new ContractService(context);

            // ACT

            var result = await service.FilterContractsAsync(
                null,
                null,
                "Active");

            // ASSERT

            Assert.Single(result);

            Assert.Equal("Active", result[0].Status);
        }
    }
}