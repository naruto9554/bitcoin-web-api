using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Services.Extensions;
using Services.Models;
using Services.Utility;

namespace Services;

public class MarketClient(ILogger<MarketClient> logger, IHttpClientFactory httpClientFactory) : IMarketClient
{
    private readonly ILogger<MarketClient> _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public async Task<List<MarketChartPoint>?> GetMarketChartByDateRange(DateOnly fromDate, DateOnly toDate)
    {
        var baseUrl = $"https://api.coingecko.com/api/v3/coins/{Constants.CryptoCurrency}/market_chart/range";
        var parameters = QueryHelper.CreateQueryParams(fromDate, toDate, Constants.Currency);
        var url = QueryHelpers.AddQueryString(baseUrl, parameters);
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        using (var httpClient = _httpClientFactory.CreateClient())
        {
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var marketChart = JsonSerializer.Deserialize<MarketChart>(json, _options);

                if (marketChart is null || marketChart.Prices.IsNullOrEmpty())
                {
                    _logger.LogInformation("Market chart data not found.");
                    return null;
                }

                var points = MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart);
                var data = MarketChartHelper.GetEarliestMarketChartPointsByDate(points);
                return data;
            }

            var exception = new HttpRequestException("Error getting market chart data", null, response.StatusCode);
            _logger.LogError(exception, "Error getting market chart data. Status: {Status}", response.StatusCode);
            throw exception;
        }
    }
}
