using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

public class UtilityTests
{
    [Fact]
    public void LongestConsecutiveDecreasingSubset()
    {
        var numberOfDays = 10;
        var now = DateTimeOffset.UtcNow;
        var marketChartPoints = new List<MarketChartPoint>();

        var price = new Random().Next(0, int.MaxValue);

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
    public void EarliestMarketChartPoints_Found()
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

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldMapCorrectly()
    {
        var marketChart = new MarketChart
        {
            Prices =
            [
                [1629811200000, 45000],
                [1629897600000, 46000]
            ],
            Market_caps =
            [
                [1629811200000, 850000000000],
                [1629897600000, 860000000000]
            ],
            Total_volumes =
            [
                [1629811200000, 35000000000],
                [1629897600000, 36000000000]
            ]
        };

        var result = MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart);

        Assert.Equal(2, result.Count);

        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1629811200000), result[0].Date);
        Assert.Equal(45000, result[0].Price);
        Assert.Equal(850000000000, result[0].MarketCap);
        Assert.Equal(35000000000, result[0].TotalVolume);

        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1629897600000), result[1].Date);
        Assert.Equal(46000, result[1].Price);
        Assert.Equal(860000000000, result[1].MarketCap);
        Assert.Equal(36000000000, result[1].TotalVolume);
    }

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldThrowException_WhenPricesAreNull()
    {
        var marketChart = new MarketChart
        {
            Prices = null,
            Market_caps = [],
            Total_volumes = []
        };

        var exception = Assert.Throws<MarketChartException>(() =>
        MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        Assert.Equal("Prices is null", exception.Message);
    }

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldThrowException_WhenMarketCapsAreNull()
    {
        var marketChart = new MarketChart
        {
            Prices = [],
            Market_caps = null,
            Total_volumes = []
        };

        var exception = Assert.Throws<MarketChartException>(() =>
        MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        Assert.Equal("Market_caps is null", exception.Message);
    }

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldThrowException_WhenTotalVolumesAreNull()
    {
        var marketChart = new MarketChart
        {
            Prices = [],
            Market_caps = [],
            Total_volumes = null
        };

        var exception = Assert.Throws<MarketChartException>(() =>
        MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        Assert.Equal("Total_volumes is null", exception.Message);
    }

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldThrowException_WhenDataLengthsAreUnequal()
    {
        var marketChart = new MarketChart
        {
            Prices =
            [
                [1629811200000, 45000]
            ],
            Market_caps =
            [
                [1629811200000, 850000000000],
                [1629897600000, 860000000000]
            ],
            Total_volumes =
            [
                [1629811200000, 35000000000]
            ]
        };

        var exception = Assert.Throws<MarketChartException>(() =>
        MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        Assert.Equal("Unequal number of data points in market chart", exception.Message);
    }

    [Fact]
    public void CreateQueryParams_ShouldReturnCorrectDictionary()
    {
        var fromDate = new DateOnly(2024, 8, 11);
        var toDate = new DateOnly(2024, 8, 12);
        var currency = "eur";
        var expectedFrom = fromDate.ToUnixTimestamp().ToString();
        var expectedTo = toDate.ToUnixTimestamp().ToString();

        var result = QueryHelper.CreateQueryParams(fromDate, toDate, currency);

        Assert.Equal(3, result.Count);
        Assert.Equal(currency, result["vs_currency"]);
        Assert.Equal(expectedFrom, result["from"]);
        Assert.Equal(expectedTo, result["to"]);
    }
}