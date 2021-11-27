using System;
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

        throw new NotImplementedException();
    }

    public async Task<TradeVolume> GetHighestTradingVolume(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);
        var highest = data.MaxBy(x => x.TotalVolume);
        return new TradeVolume
        {
            Date = DateHelper.DateTimeOffsetToDate(highest.Date),
            Volume = highest.TotalVolume
        };
    }

    public async Task<DateRange> GetBuyAndSellDates(string fromDate, string toDate)
    {
        var data = await _marketStore.GetMarketChartByDateRange(fromDate, toDate);


        throw new NotImplementedException();
    }
}