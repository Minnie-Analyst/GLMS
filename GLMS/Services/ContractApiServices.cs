using GLMS.Models;
using System.Net.Http.Json;

namespace GLMS.Services
{
    public class ContractApiService
    {
        private readonly HttpClient _httpClient;

        public ContractApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Contract>> GetContractsAsync()
        {
            var contracts = await _httpClient
                .GetFromJsonAsync<List<Contract>>("api/contracts");

            return contracts ?? new List<Contract>();
        }
    }
}