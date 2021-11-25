using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class MarketService : IMarketService
{
    private readonly ILogger<MarketService> _logger;
    private readonly IMarketStore _marketStore;

    public MarketService(ILogger<MarketService> logger, IMarketStore marketStore)
    {
        _logger = logger;
        _marketStore = marketStore;
    }

    public async Task<int> GetLongestDownwardTrend(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);
        return data.Prices.Length;
    }

    public async Task<string> GetHighestTradingVolumeDate(string fromDate, string toDate)
    {
        throw new NotImplementedException();
    }

    public async Task<(string BuyDate, string SellDate)> GetBuyAndSellDates(string fromDate, string toDate)
    {
        throw new NotImplementedException();
    }
}