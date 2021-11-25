using System.Threading.Tasks;

public interface IMarketStore
{
    Task<MarketChart> GetMarketChartByDateRange(string fromDate, string toDate);
}