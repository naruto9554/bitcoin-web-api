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
        _logger.LogInformation($"Getting longest downward trend for price from {fromDate} to {toDate}");
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);

        if (data == null) return null;

        var longestDownwardPriceTrend = ListHelper.LongestConsecutiveDecreasingSubset(data.Select(x => x.Price).ToList());
        _logger.LogInformation($"Longest downward price trend {longestDownwardPriceTrend} days.");
        return longestDownwardPriceTrend;
    }

    public async Task<TradeVolume?> GetHighestTradingVolume(string fromDate, string toDate)
    {
        _logger.LogInformation($"Getting highest trading volume and date from {fromDate} to {toDate}");
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);

        if (data == null) return null;

        var highestByTotalVolume = data.MaxBy(x => x.TotalVolume);

        if (highestByTotalVolume == null) return null;

        var tradeVolume = new TradeVolume
        {
            Date = DateHelper.DateTimeOffsetToDate(highestByTotalVolume.Date),
            Volume = highestByTotalVolume.TotalVolume,
        };

        _logger.LogInformation($"Highest trade volume {tradeVolume.Volume} on {tradeVolume.Date}.");
        return tradeVolume;
    }

    public async Task<TradeDate?> GetBestBuyAndSellDates(string fromDate, string toDate)
    {
        _logger.LogInformation($"Getting best buy and sell dates from {fromDate} to {toDate}");
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);

        if (data == null) return null;

        var priceIsOnlyDecreasing = ListHelper.IsOnlyDecreasing(data.Select(x => x.Price).ToList());

        if (priceIsOnlyDecreasing) return null;

        var lowestByPrice = data.MinBy(x => x.Price);
        var highestByPrice = data.MaxBy(x => x.Price);

        if (lowestByPrice == null || highestByPrice == null) return null;

        var tradeDate = new TradeDate
        {
            SellDate = DateHelper.DateTimeOffsetToDate(highestByPrice.Date),
            BuyDate = DateHelper.DateTimeOffsetToDate(lowestByPrice.Date),
        };

        _logger.LogInformation($"Best buy date {tradeDate.BuyDate} and best sell date {tradeDate.SellDate}.");
        return tradeDate;
    }
}