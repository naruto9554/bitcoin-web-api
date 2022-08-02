using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

public class HelperTests
{
    [Fact]
    public void TestLongestConsecutiveDecreasingSubset()
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
        var longest = prices.LongestConsecutiveDecreasingSubset();

        prices.Should().NotBeEmpty();
        prices.Count.Should().Be(numberOfDays + 2);
        longest.Should().Be(numberOfDays);
    }

    [Fact]
    public void TestEarliestMarketChartPointsFound()
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

        result.Should().NotBeEmpty();
        if (now.TimeOfDay.Ticks == 0)
        {
            result.Count.Should().Be(numberOfDays);
        }
        else
        {
            result.Count.Should().Be(numberOfDays + 1);
        }
    }
}