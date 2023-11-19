using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

public class MarketClient : IMarketClient
{
    private readonly ILogger<MarketClient> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public MarketClient(ILogger<MarketClient> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

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
                _logger.LogInformation("Successfully found market chart data. Points: {count}", data.Count);
                return data;
            }

            var exception = new HttpRequestException(response.StatusCode.ToString());
            _logger.LogError(exception, "Error getting market chart data");
            throw exception;
        }
    }
}