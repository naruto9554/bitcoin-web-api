using System.Net;
using FluentAssertions;
using Xunit;

public class IntegrationTests(IntegrationFixture fixture) : IClassFixture<IntegrationFixture>
{
    private const string BaseUrl = "/api/v1";
    private readonly IntegrationFixture _fixture = fixture;

    public static TheoryData<string?, string?, HttpStatusCode> Cases =>
    new TheoryData<string?, string?, HttpStatusCode>
    {
            {DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), HttpStatusCode.OK},
            {DateTime.Now.AddMonths(-13).ToString("yyyy-MM-dd"), DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"), HttpStatusCode.OK}, //No longer unauthorized for over 365 days old queries
            {"", null, HttpStatusCode.BadRequest},
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task LongestDownwardTrend(string? fromDate, string? toDate, HttpStatusCode status)
    {
        var result = await _fixture.Client.GetAsync($"{BaseUrl}/longestdownwardtrend?fromDate={fromDate}&toDate={toDate}");
        result.StatusCode.Should().BeOneOf(status, HttpStatusCode.TooManyRequests);
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task HighestTradingVolume(string? fromDate, string? toDate, HttpStatusCode status)
    {
        var result = await _fixture.Client.GetAsync($"{BaseUrl}/highestradingvolume?fromDate={fromDate}&toDate={toDate}");
        result.StatusCode.Should().BeOneOf(status, HttpStatusCode.TooManyRequests);
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task BuyAndSell(string? fromDate, string? toDate, HttpStatusCode status)
    {
        var result = await _fixture.Client.GetAsync($"{BaseUrl}/buyandsell?fromDate={fromDate}&toDate={toDate}");
        result.StatusCode.Should().BeOneOf(status, HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task Swagger()
    {
        var result = await _fixture.Client.GetAsync("/");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Health()
    {
        var result = await _fixture.Client.GetAsync("/health");
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

