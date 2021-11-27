using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMarketStore
{
    Task<List<MarketChartPoint>> GetMarketChartByDateRange(string fromDate, string toDate);
}