using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class HelperTests
{
    [Fact]
    public void ASD()
    {
        var now = DateTimeOffset.UtcNow;
        var marketChartPoints = new List<MarketChartPoint>();

        for (var i = 0; i < 3; i++)
        {
            marketChartPoints.Add(new MarketChartPoint
            {
                Date = now.AddDays(i),
                Price = 1000 + i,
                MarketCap = new Random().Next(),
                TotalVolume = new Random().Next(),
            });
        }

        for (var i = 1; i < 5; i++)
        {
            marketChartPoints.Add(new MarketChartPoint
            {
                Date = now.AddDays(i),
                Price = 1000 - i,
                MarketCap = new Random().Next(),
                TotalVolume = new Random().Next(),
            });
        }

        var prices = marketChartPoints.Select(x => x.Price).ToList();
        var longest = ListHelper.LongestConsecutiveDecreasingSubset(prices);

        Assert.Equal(4, longest);
    }

    [Fact]
    public void EarliestMarketChartPointsFound()
    {
        var numberOfDays = 5;
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