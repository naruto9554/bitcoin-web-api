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

    public async Task<int?> GetLongestDownwardTrend(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);

        if (data == null) return null;

        var longestDownwardPrice = ListHelper.LongestConsecutiveDecreasingSubset(data.Select(x => x.Price).ToList());
        return longestDownwardPrice;
    }

    public async Task<TradeVolume?> GetHighestTradingVolume(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);

        if (data == null) return null;

        var highestByTotalVolume = data.MaxBy(x => x.TotalVolume);

        if (highestByTotalVolume == null) return null;

        return new TradeVolume
        {
            Date = DateHelper.DateTimeOffsetToDate(highestByTotalVolume.Date),
            Volume = highestByTotalVolume.TotalVolume,
        };
    }

    public async Task<TradeDate?> GetBestBuyAndSellDates(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);

        if (data == null) return null;

        var priceIsOnlyDecreasing = ListHelper.IsOnlyDecreasing(data.Select(x => x.Price).ToList());

        if (priceIsOnlyDecreasing) return null;

        var lowestByPrice = data.MinBy(x => x.Price);
        var highestByPrice = data.MaxBy(x => x.Price);

        if (lowestByPrice == null || highestByPrice == null) return null;

        return new TradeDate
        {
            SellDate = DateHelper.DateTimeOffsetToDate(highestByPrice.Date),
            BuyDate = DateHelper.DateTimeOffsetToDate(lowestByPrice.Date),
        };
    }
}