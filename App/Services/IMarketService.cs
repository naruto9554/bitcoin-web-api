using System;
using System.Threading.Tasks;

public interface IMarketService
{
    Task<int?> GetLongestDownwardTrend(DateOnly fromDate, DateOnly toDate);
    Task<(string Date, decimal Volume)?> GetHighestTradingVolume(DateOnly fromDate, DateOnly toDate);
    Task<(string SellDate, string BuyDate)?> GetBestBuyAndSellDates(DateOnly fromDate, DateOnly toDate);
}