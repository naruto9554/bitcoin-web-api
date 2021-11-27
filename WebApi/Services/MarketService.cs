using System;
using System.Collections.Generic;
using System.Linq;
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
        var longestDownwardPrice = ListHelper.LongestConsecutiveDecreasingSubset(data.Select(x => x.Price).ToList());
        return longestDownwardPrice;
    }

    public async Task<TradeVolume> GetHighestTradingVolume(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);
        var highestByTotalVolume = data.MaxBy(x => x.TotalVolume);
        return new TradeVolume
        {
            Date = DateHelper.DateTimeOffsetToDate(highestByTotalVolume.Date),
            Volume = highestByTotalVolume.TotalVolume,
        };
    }

    public async Task<TradeDate> GetBestBuyAndSellDates(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);

        var priceIsOnlyDecreasing = ListHelper.IsOnlyDecreasing(data.Select(x => x.Price).ToList());

        if (priceIsOnlyDecreasing)
        {
            return new TradeDate
            {
                SellDate = null,
                BuyDate = null,
            };
        }

        var lowestByPrice = data.MinBy(x => x.Price);
        var highestByPrice = data.MaxBy(x => x.Price);

        return new TradeDate
        {
            SellDate = DateHelper.DateTimeOffsetToDate(highestByPrice.Date),
            BuyDate = DateHelper.DateTimeOffsetToDate(lowestByPrice.Date),
        };
    }
}