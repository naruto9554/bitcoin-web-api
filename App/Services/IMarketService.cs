using System;
using System.Threading.Tasks;

public interface IMarketService
{
    Task<int?> GetLongestDownwardTrend(DateOnly fromDate, DateOnly toDate);
    Task<(DateOnly Date, decimal Volume)?> GetHighestTradingVolume(DateOnly fromDate, DateOnly toDate);
    Task<(DateOnly SellDate, DateOnly BuyDate)?> GetBestBuyAndSellDates(DateOnly fromDate, DateOnly toDate);
}