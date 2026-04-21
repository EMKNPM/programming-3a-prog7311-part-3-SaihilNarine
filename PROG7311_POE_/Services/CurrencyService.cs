using System.Text.Json;

namespace PROG7311_POE_.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertUsdToZar(decimal usd)
        {
            var response = await _httpClient.GetStringAsync(
                "https://api.exchangerate-api.com/v4/latest/USD");

            using var doc = JsonDocument.Parse(response);

            var rate = doc.RootElement
                          .GetProperty("rates")
                          .GetProperty("ZAR")
                          .GetDecimal();

            return usd * rate;
        }
    }
}
