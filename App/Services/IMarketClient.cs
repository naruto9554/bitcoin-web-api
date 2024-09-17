public interface IMarketClient
{
    Task<List<MarketChartPoint>?> GetMarketChartByDateRange(DateOnly fromDate, DateOnly toDate);
}