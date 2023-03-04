using System.Threading.Tasks;

public interface IMarketService
{
    Task<int?> GetLongestDownwardTrend(string fromDate, string toDate);
    Task<(string Date, decimal Volume)?> GetHighestTradingVolume(string fromDate, string toDate);
    Task<(string SellDate, string BuyDate)?> GetBestBuyAndSellDates(string fromDate, string toDate);
}