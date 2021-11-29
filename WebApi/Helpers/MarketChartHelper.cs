using System;
using System.Collections.Generic;
using System.Linq;

public static class MarketChartHelper
{
    public static List<MarketChartPoint> MapMarketChartToMarketChartPoints(MarketChart marketChart)
    {
        if (marketChart.Prices == null)
        {
            throw new ArgumentNullException(nameof(marketChart.Prices));
        }

        if (marketChart.Market_caps == null)
        {
            throw new ArgumentNullException(nameof(marketChart.Market_caps));
        }

        if (marketChart.Total_volumes == null)
        {
            throw new ArgumentNullException(nameof(marketChart.Total_volumes));
        }

        if (marketChart.Prices.Length != marketChart.Market_caps.Length &&
            marketChart.Prices.Length != marketChart.Total_volumes.Length)
        {
            throw new Exception("Unequal number of data points in market chart");
        }

        var marketChartPoints = new List<MarketChartPoint>();
        for (var i = 0; i < marketChart.Prices.Length; i++)
        {
            var marketData = new MarketChartPoint();
            marketData.Date = DateHelper.UnixTimeToDateTimeOffset((long)marketChart.Prices[i][0]);
            marketData.Price = marketChart.Prices[i][1];
            marketData.MarketCap = marketChart.Market_caps[i][1];
            marketData.TotalVolume = marketChart.Total_volumes[i][1];
            marketChartPoints.Add(marketData);
        }
        return marketChartPoints;
    }

    public static List<MarketChartPoint> GetEarliestMarketChartPointsByDate(List<MarketChartPoint> marketChartPoints)
    {
        var grouped = marketChartPoints.GroupBy(x => new
        { y = x.Date.Year, m = x.Date.Month, d = x.Date.Day }).Select(x => new
        {
            Date = x.Key,
            Data = x.Select(y => new MarketChartPoint
            {
                Date = y.Date,
                Price = y.Price,
                MarketCap = y.MarketCap,
                TotalVolume = y.TotalVolume,
            }).ToList(),
        }).ToList();

        var earliestMarketChartPoints = grouped.Select(x =>
        {
            var earliest = x.Data.MinBy(y => y.Date);

            if (earliest == null)
            {
                throw new ArgumentNullException(nameof(earliest));
            }

            return new MarketChartPoint
            {
                Date = earliest.Date,
                Price = earliest.Price,
                MarketCap = earliest.MarketCap,
                TotalVolume = earliest.TotalVolume,
            };
        }).ToList();

        return earliestMarketChartPoints;
    }
}