using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMarketClient
{
    Task<List<MarketChartPoint>?> GetMarketChartByDateRange(DateOnly fromDate, DateOnly toDate);
}