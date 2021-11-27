using System.Threading.Tasks;

public interface IMarketService
{
    Task<int> GetLongestDownwardTrend(string fromDate, string toDate);
    Task<TradeVolume> GetHighestTradingVolume(string fromDate, string toDate);
    Task<TradeDate> GetBestBuyAndSellDates(string fromDate, string toDate);
}