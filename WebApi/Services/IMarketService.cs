using System.Threading.Tasks;

public interface IMarketService
{
    Task<int> GetLongestDownwardTrend(string fromDate, string toDate);
    Task<string> GetHighestTradingVolumeDate(string fromDate, string toDate);
    Task<(string BuyDate, string SellDate)> GetBuyAndSellDates(string fromDate, string toDate);
}