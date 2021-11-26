using System;
using System.Collections.Generic;

public static class MarketMapper
{
    public static List<MarketData> MapMarketChartToMarketData(MarketChart marketChart)
    {
        var marketDataList = new List<MarketData>();
        for (var i = 0; i < marketChart.Prices.Length; i++)
        {
            var marketData = new MarketData();
            marketData.Date = (long)marketChart.Prices[i][0];
            marketData.Price = marketChart.Prices[i][1];
            marketData.MarketCap = marketChart.Market_caps[i][1];
            marketData.TotalVolume = marketChart.Total_volumes[i][1];
            marketDataList.Add(marketData);
        }

        Console.WriteLine(marketDataList.Count);
        return marketDataList;
    }
}