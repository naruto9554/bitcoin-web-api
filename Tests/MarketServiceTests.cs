using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Xunit;

public class MarketServiceTests
{
    public readonly string FromDate;
    public readonly string ToDate;
    public readonly string ToDateExtension;
    public readonly string ToDateNullExtension;

    public readonly IMarketService _marketService;

    public MarketServiceTests()
    {
        var date = new DateTimeOffset(2021, 1, 1, 12, 0, 0, 0, new TimeSpan(0, 0, 0));

        var marketChartPoints = new List<MarketChartPoint> {
            new() {
                Date = date,
                Price = 100m,
                MarketCap = 100m,
                TotalVolume = 100m,
            },
            new() {
                Date = date.AddDays(1),
                Price = 90m,
                MarketCap = 100m,
                TotalVolume = 200m,
            },
            new()
            {
                Date = date.AddDays(2),
                Price = 80m,
                MarketCap = 100m,
                TotalVolume = 300m,
            },
            new() {
                Date = date.AddDays(3),
                Price = 70m,
                MarketCap = 100m,
                TotalVolume = 400m,
            },
            new() {
                Date = date.AddDays(4),
                Price = 500m,
                MarketCap = 100m,
                TotalVolume = 500m,
            },
        };

        var marketChartPointsExtension = new List<MarketChartPoint> {
            new() {
                Date = date.AddDays(5),
                Price = 50m,
                MarketCap = 100m,
                TotalVolume = 50m,
            },
            new() {
                Date = date.AddDays(6),
                Price = 40m,
                MarketCap = 100m,
                TotalVolume = 40m,
            },
            new() {
                Date = date.AddDays(7),
                Price = 30m,
                MarketCap = 100m,
                TotalVolume = 30m,
            },
        };

        List<MarketChartPoint>? marketChartPointsNullExtension = null;

        var logger = new NullLogger<MarketService>();

        FromDate = date.ToString(Constants.DateFormat);
        ToDate = date.AddDays(4).ToString(Constants.DateFormat);
        ToDateExtension = date.AddDays(7).ToString(Constants.DateFormat);
        ToDateNullExtension = date.AddDays(10).ToString(Constants.DateFormat);

        var marketClient = Substitute.For<IMarketClient>();
        marketClient.GetMarketChartByDateRange(FromDate, ToDate)!
            .Returns(Task.FromResult(marketChartPoints));
        marketClient.GetMarketChartByDateRange(ToDate, ToDateExtension)!
            .Returns(Task.FromResult(marketChartPointsExtension));
        marketClient.GetMarketChartByDateRange(ToDateExtension, ToDateNullExtension)
            .Returns(Task.FromResult(marketChartPointsNullExtension));

        _marketService = new MarketService(logger, marketClient);
    }

    [Fact]
    public async Task TestLongestDownwardTrendReturnsValue()
    {
        var result = await _marketService.GetLongestDownwardTrend(FromDate, ToDate);

        result.Should().NotBeNull();
        result.Should().Be(3);
    }

    [Fact]
    public async Task TestLongestDownwardTrendNoDataReturnsNull()
    {
        var result = await _marketService.GetLongestDownwardTrend(ToDateExtension, ToDateNullExtension);

        result.Should().BeNull();
    }

    [Fact]
    public async Task TestHighestTradingVolumeReturnsValue()
    {
        var result = await _marketService.GetHighestTradingVolume(FromDate, ToDate);

        result.Should().NotBeNull();
        result?.Date.Should().Be("2021-01-05");
        result?.Volume.Should().Be(500m);
    }

    [Fact]
    public async Task TestHighestTradingVolumeNoDataReturnsNull()
    {
        var result = await _marketService.GetHighestTradingVolume(ToDateExtension, ToDateNullExtension);

        result.Should().BeNull();
    }

    [Fact]
    public async Task TestBestBuyAndSellDatesReturnsValue()
    {
        var result = await _marketService.GetBestBuyAndSellDates(FromDate, ToDate);

        result.Should().NotBeNull();
        result?.BuyDate.Should().Be("2021-01-04");
        result?.SellDate.Should().Be("2021-01-05");
    }

    [Fact]
    public async Task TestBestBuyAndSellDatesOnlyDecreasingReturnsNull()
    {
        var result = await _marketService.GetBestBuyAndSellDates(ToDate, ToDateExtension);

        result.Should().BeNull();
    }

    [Fact]
    public async Task TestBestBuyAndSellDatesNoDataReturnsNull()
    {
        var result = await _marketService.GetBestBuyAndSellDates(ToDateExtension, ToDateNullExtension);

        result.Should().BeNull();
    }
}