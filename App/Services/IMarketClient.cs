using Services.Models;

namespace Services;

public interface IMarketClient
{
    Task<List<MarketChartPoint>?> GetMarketChartByDateRange(DateOnly fromDate, DateOnly toDate);
}