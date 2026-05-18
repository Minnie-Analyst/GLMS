using Newtonsoft.Json;

namespace GLMS.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetUsdToZarRate()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(
                    "https://api.exchangerate-api.com/v4/latest/USD");

                dynamic data = JsonConvert.DeserializeObject(response);

                decimal rate = (decimal)data.rates.ZAR;

                // ROUND TO 2 DECIMALS
                return Math.Round(rate, 2);
            }
            catch
            {
                // FALLBACK RATE IF API FAILS
                return 18.50m;
            }
        }
    }
}