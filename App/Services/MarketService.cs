using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class MarketService : IMarketService
{
    private readonly ILogger<MarketService> _logger;
    private readonly IMarketClient _marketClient;

    public MarketService(ILogger<MarketService> logger, IMarketClient marketClient)
    {
        _logger = logger;
        _marketClient = marketClient;
    }

    public async Task<int?> GetLongestDownwardTrend(string fromDate, string toDate)
    {
        _logger.LogInformation("Getting longest downward trend for price from {fromDate} to {toDate}", fromDate, toDate);
        var data = await _marketClient.GetMarketChartByDateRange(fromDate, toDate);

        if (data is null) return null;

        var prices = data.Select(x => x.Price).ToList();
        var longestDownwardPriceTrend = prices.LongestConsecutiveDecreasingSubset();
        _logger.LogInformation("Longest downward price trend {longestDownwardPriceTrend} days.", longestDownwardPriceTrend);
        return longestDownwardPriceTrend;
    }

    public async Task<(string Date, decimal Volume)?> GetHighestTradingVolume(string fromDate, string toDate)
    {
        _logger.LogInformation("Getting highest trading volume and date from {fromDate} to {toDate}", fromDate, toDate);
        var data = await _marketClient.GetMarketChartByDateRange(fromDate, toDate);

        if (data is null) return null;

        var highestByTotalVolume = data.MaxBy(x => x.TotalVolume);

        if (highestByTotalVolume is null) return null;

        var trade = (
            Date: highestByTotalVolume.Date.ToString(Constants.DateFormat),
            Volume: highestByTotalVolume.TotalVolume
            );

        _logger.LogInformation("Highest trade volume {volume} on {date}.", trade.Volume, trade.Date);
        return trade;
    }

    public async Task<(string SellDate, string BuyDate)?> GetBestBuyAndSellDates(string fromDate, string toDate)
    {
        _logger.LogInformation("Getting best buy and sell dates from {fromDate} to {toDate}", fromDate, toDate);
        var data = await _marketClient.GetMarketChartByDateRange(fromDate, toDate);

        if (data is null) return null;

        var prices = data.Select(x => x.Price).ToList();
        var priceIsOnlyDecreasing = prices.IsOrderedDecreasing();

        if (priceIsOnlyDecreasing) return null;

        var lowestByPrice = data.MinBy(x => x.Price);
        var highestByPrice = data.MaxBy(x => x.Price);

        if (lowestByPrice is null || highestByPrice is null) return null;

        var trade = (
            SellDate: highestByPrice.Date.ToString(Constants.DateFormat),
            BuyDate: lowestByPrice.Date.ToString(Constants.DateFormat)
        );

        _logger.LogInformation("Best buy date {buyDate} and best sell date {sellDate}.", trade.BuyDate, trade.SellDate);
        return trade;
    }
}