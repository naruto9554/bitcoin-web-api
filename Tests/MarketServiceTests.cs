using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

public class MarketServiceTests
{
    public readonly string FromDate;
    public readonly string ToDate;
    public readonly IMarketService _marketService;
    public readonly List<MarketChartPoint>? _marketChartPoints;

    public MarketServiceTests()
    {
        var date = new DateTimeOffset(2021, 1, 1, 12, 0, 0, 0, new TimeSpan(0, 0, 0));

        FromDate = DateHelper.DateTimeOffsetToDate(date);
        ToDate = DateHelper.DateTimeOffsetToDate(date.AddDays(4));

        _marketChartPoints = new List<MarketChartPoint> {
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

        var logger = new NullLogger<MarketService>();

        var marketStore = new Mock<IMarketStore>(MockBehavior.Strict);
        marketStore.Setup(x => x.GetMarketChartByDateRange(FromDate, ToDate))
            .ReturnsAsync(_marketChartPoints);

        _marketService = new MarketService(logger, marketStore.Object);
    }

    [Fact]
    public async Task TestLongestDownwardTrendReturnsValue()
    {
        var result = await _marketService.GetLongestDownwardTrend(FromDate, ToDate);

        Assert.NotNull(result);
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task TestHighestTradingVolumeReturnsValue()
    {
        var result = await _marketService.GetHighestTradingVolume(FromDate, ToDate);

        Assert.NotNull(result);
        Assert.NotNull(result?.Date);
        Assert.NotNull(result?.Volume);
        Assert.Equal("2021-01-05", result?.Date);
        Assert.Equal(500m, result?.Volume);
    }

    [Fact]
    public async Task TestBestBuyAndSellDatesReturnsValue()
    {
        var result = await _marketService.GetBestBuyAndSellDates(FromDate, ToDate);

        Assert.NotNull(result);
        Assert.NotNull(result?.BuyDate);
        Assert.NotNull(result?.SellDate);
        Assert.Equal("2021-01-04", result?.BuyDate);
        Assert.Equal("2021-01-05", result?.SellDate);
    }
}