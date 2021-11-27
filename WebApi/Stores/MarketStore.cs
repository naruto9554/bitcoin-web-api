using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class MarketStore : IMarketStore
{
    private const string BaseUrl = "https://api.coingecko.com/api/v3";
    private readonly ILogger<MarketStore> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public MarketStore(ILogger<MarketStore> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<MarketChartPoint>> GetMarketChartByDateRange(string fromDate, string toDate)
    {
        var id = "bitcoin";
        var vs_currency = "eur";
        var from = DateHelper.DateToUnixTime(fromDate).ToString();
        var to = DateHelper.DateToUnixTime(toDate).ToString();

        var parameters = new List<KeyValuePair<string, string>>();
        parameters.Add(new KeyValuePair<string, string>(nameof(vs_currency), vs_currency));
        parameters.Add(new KeyValuePair<string, string>(nameof(from), from));
        parameters.Add(new KeyValuePair<string, string>(nameof(to), to));

        var query = QueryString.Create(parameters);
        var url = $"{BaseUrl}/coins/{id}/market_chart/range{query.Value}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully found market chart data");
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var marketChart = JsonSerializer.Deserialize<MarketChart>(json, options);
                var data = MarketDataMapper.MapMarketChartToMarketChartPoints(marketChart);
                return data;
            }

            var exception = new HttpRequestException(response.StatusCode.ToString());
            _logger.LogError("Error getting market chart data", exception);
            throw exception;
        }
    }
}