using System.Globalization;
using Services.Exceptions;
using Services.Extensions;
using Services.Models;
using Services.Utility;
using Shouldly;
using Xunit;

namespace UnitTests;

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

        prices.ShouldNotBeEmpty();
        prices.Count.ShouldBe(numberOfDays + 2);
        longest.ShouldBe(numberOfDays);
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

        result.ShouldNotBeEmpty();
        if (now.TimeOfDay.Ticks == 0)
        {
            result.Count.ShouldBe(numberOfDays);
        }
        else
        {
            result.Count.ShouldBe(numberOfDays + 1);
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
            MarketCaps =
            [
                [1629811200000, 850000000000],
            [1629897600000, 860000000000]
            ],
            TotalVolumes =
            [
                [1629811200000, 35000000000],
            [1629897600000, 36000000000]
            ]
        };

        var result = MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart);

        result.Count.ShouldBe(2);

        result[0].Date.ShouldBe(DateTimeOffset.FromUnixTimeMilliseconds(1629811200000));
        result[0].Price.ShouldBe(45000);
        result[0].MarketCap.ShouldBe(850000000000);
        result[0].TotalVolume.ShouldBe(35000000000);

        result[1].Date.ShouldBe(DateTimeOffset.FromUnixTimeMilliseconds(1629897600000));
        result[1].Price.ShouldBe(46000);
        result[1].MarketCap.ShouldBe(860000000000);
        result[1].TotalVolume.ShouldBe(36000000000);
    }

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldThrowException_WhenPricesAreNull()
    {
        var marketChart = new MarketChart
        {
            Prices = null,
            MarketCaps = [],
            TotalVolumes = []
        };

        var exception = Should.Throw<MarketChartException>(() =>
            MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        exception.Message.ShouldBe("Prices is null");
    }

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldThrowException_WhenMarketCapsAreNull()
    {
        var marketChart = new MarketChart
        {
            Prices = [],
            MarketCaps = null,
            TotalVolumes = []
        };

        var exception = Should.Throw<MarketChartException>(() =>
            MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        exception.Message.ShouldBe("MarketCaps is null");
    }

    [Fact]
    public void MapMarketChartToMarketChartPoints_ShouldThrowException_WhenTotalVolumesAreNull()
    {
        var marketChart = new MarketChart
        {
            Prices = [],
            MarketCaps = [],
            TotalVolumes = null
        };

        var exception = Should.Throw<MarketChartException>(() =>
            MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        exception.Message.ShouldBe("TotalVolumes is null");
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
            MarketCaps =
            [
                [1629811200000, 850000000000],
            [1629897600000, 860000000000]
            ],
            TotalVolumes =
            [
                [1629811200000, 35000000000]
            ]
        };

        var exception = Should.Throw<MarketChartException>(() =>
            MarketChartHelper.MapMarketChartToMarketChartPoints(marketChart));

        exception.Message.ShouldBe("Unequal number of data points in market chart");
    }

    [Fact]
    public void CreateQueryParams_ShouldReturnCorrectDictionary()
    {
        var fromDate = new DateOnly(2024, 8, 11);
        var toDate = new DateOnly(2024, 8, 12);
        const string currency = "eur";
        var expectedFrom = fromDate.ToUnixTimestamp().ToString(CultureInfo.InvariantCulture);
        var expectedTo = toDate.ToUnixTimestamp().ToString(CultureInfo.InvariantCulture);

        var result = QueryHelper.CreateQueryParams(fromDate, toDate, currency);

        result.Count.ShouldBe(3);
        result["vs_currency"].ShouldBe(currency);
        result["from"].ShouldBe(expectedFrom);
        result["to"].ShouldBe(expectedTo);
    }
}
