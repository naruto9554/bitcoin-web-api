using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
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
            new MarketChartPoint
            {
                Date = date,
                Price = 100m,
                MarketCap = 100m,
                TotalVolume = 100m,
            },
            new MarketChartPoint
            {
                Date = date.AddDays(1),
                Price = 90m,
                MarketCap = 100m,
                TotalVolume = 200m,
            },
            new MarketChartPoint
            {
                Date = date.AddDays(2),
                Price = 80m,
                MarketCap = 100m,
                TotalVolume = 300m,
            },
                new MarketChartPoint
            {
                Date = date.AddDays(3),
                Price = 70m,
                MarketCap = 100m,
                TotalVolume = 400m,
            },
                new MarketChartPoint
            {
                Date = date.AddDays(4),
                Price = 500m,
                MarketCap = 100m,
                TotalVolume = 500m,
            },
        };

        var marketChartPointsExtension = new List<MarketChartPoint> {
            new MarketChartPoint
            {
                Date = date.AddDays(5),
                Price = 50m,
                MarketCap = 100m,
                TotalVolume = 50m,
            },
            new MarketChartPoint
            {
                Date = date.AddDays(6),
                Price = 40m,
                MarketCap = 100m,
                TotalVolume = 40m,
            },
            new MarketChartPoint
            {
                Date = date.AddDays(7),
                Price = 30m,
                MarketCap = 100m,
                TotalVolume = 30m,
            },
        };

        List<MarketChartPoint>? marketChartPointsNullExtension = null;

        var logger = new NullLogger<MarketService>();

        FromDate = DateHelper.DateTimeOffsetToDate(date);
        ToDate = DateHelper.DateTimeOffsetToDate(date.AddDays(4));
        ToDateExtension = DateHelper.DateTimeOffsetToDate(date.AddDays(7));
        ToDateNullExtension = DateHelper.DateTimeOffsetToDate(date.AddDays(10));

        var marketStore = new Mock<IMarketStore>(MockBehavior.Strict);
        marketStore.Setup(x => x.GetMarketChartByDateRange(FromDate, ToDate))
            .ReturnsAsync(marketChartPoints);
        marketStore.Setup(x => x.GetMarketChartByDateRange(ToDate, ToDateExtension))
            .ReturnsAsync(marketChartPointsExtension);
        marketStore.Setup(x => x.GetMarketChartByDateRange(ToDateExtension, ToDateNullExtension))
            .ReturnsAsync(marketChartPointsNullExtension);

        _marketService = new MarketService(logger, marketStore.Object);
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