using Services.Exceptions;
using Services.Models;

namespace Services.Utility;

public static class MarketChartHelper
{
    public static List<MarketChartPoint> MapMarketChartToMarketChartPoints(MarketChart marketChart)
    {
        if (marketChart.Prices is null)
        {
            throw new MarketChartException($"{nameof(marketChart.Prices)} is null");
        }

        if (marketChart.MarketCaps is null)
        {
            throw new MarketChartException($"{nameof(marketChart.MarketCaps)} is null");
        }

        if (marketChart.TotalVolumes is null)
        {
            throw new MarketChartException($"{nameof(marketChart.TotalVolumes)} is null");
        }

        if (marketChart.Prices.Length != marketChart.MarketCaps.Length ||
            marketChart.Prices.Length != marketChart.TotalVolumes.Length)
        {
            throw new MarketChartException("Unequal number of data points in market chart");
        }

        var marketChartPoints = new List<MarketChartPoint>();
        for (var i = 0; i < marketChart.Prices.Length; i++)
        {
            var marketData = new MarketChartPoint
            {
                Date = DateTimeOffset.FromUnixTimeMilliseconds((long)marketChart.Prices[i][0]),
                Price = marketChart.Prices[i][1],
                MarketCap = marketChart.MarketCaps[i][1],
                TotalVolume = marketChart.TotalVolumes[i][1]
            };
            marketChartPoints.Add(marketData);
        }
        return marketChartPoints;
    }

    public static List<MarketChartPoint> GetEarliestMarketChartPointsByDate(List<MarketChartPoint> marketChartPoints)
    {
        var grouped = marketChartPoints
            .GroupBy(x => new
            {
                x.Date.Year,
                x.Date.Month,
                x.Date.Day
            })
            .Select(x => new
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

        var earliestMarketChartPoints = grouped
            .Select(x =>
            {
                var earliest = x.Data.MinBy(y => y.Date);

                if (earliest is null)
                {
                    return new MarketChartPoint();
                }

                return new MarketChartPoint
                {
                    Date = earliest.Date,
                    Price = earliest.Price,
                    MarketCap = earliest.MarketCap,
                    TotalVolume = earliest.TotalVolume,
                };
            })
            .ToList();

        return earliestMarketChartPoints;
    }
}