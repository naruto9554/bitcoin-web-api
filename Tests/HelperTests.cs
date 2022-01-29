using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class HelperTests
{
    [Fact]
    public void LongestConsecutiveDecreasingSubset()
    {
        var numberOfDays = 10;
        var now = DateTimeOffset.UtcNow;
        var marketChartPoints = new List<MarketChartPoint>();

        var price = new Random().Next(0, Int32.MaxValue);

        marketChartPoints.Add(new MarketChartPoint
        {
            Date = now,
            Price = price,
            MarketCap = new Random().Next(),
            TotalVolume = new Random().Next(),
        });

        for (var i = 1; i <= numberOfDays; i++)
        {
            marketChartPoints.Add(new MarketChartPoint
            {
                Date = now.AddDays(i),
                Price = price - i,
                MarketCap = new Random().Next(),
                TotalVolume = new Random().Next(),
            });
        }

        marketChartPoints.Add(new MarketChartPoint
        {
            Date = now.AddDays(numberOfDays + 1),
            Price = price + 1,
            MarketCap = new Random().Next(),
            TotalVolume = new Random().Next(),
        });

        var prices = marketChartPoints.Select(x => x.Price).ToList();
        var longest = ListHelper.LongestConsecutiveDecreasingSubset(prices);

        Assert.Equal(numberOfDays + 2, prices.Count);
        Assert.Equal(numberOfDays, longest);
    }

    [Fact]
    public void EarliestMarketChartPointsFound()
    {
        var numberOfDays = 10;
        var now = DateTimeOffset.UtcNow;
        var marketChartPoints = new List<MarketChartPoint>();
        for (var i = 1; i < 24 * numberOfDays; i++)
        {
            marketChartPoints.Add(new MarketChartPoint
            {
                Date = now.AddHours(i),
                Price = new Random().Next(),
                MarketCap = new Random().Next(),
                TotalVolume = new Random().Next(),
            });
        }

        var result = MarketChartHelper.GetEarliestMarketChartPointsByDate(marketChartPoints);

        if (now.TimeOfDay.Ticks == 0)
        {
            Assert.Equal(numberOfDays, result.Count);
        }
        else
        {
            Assert.Equal(numberOfDays + 1, result.Count);
        }
    }
}