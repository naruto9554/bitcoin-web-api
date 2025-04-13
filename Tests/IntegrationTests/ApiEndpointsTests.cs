using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Setup;
using Shouldly;
using Xunit;

namespace IntegrationTests;

public class ApiEndpointsTests(ApiFixture fixture) : IClassFixture<ApiFixture>
{
    private const string BaseUrl = "/api/v1";
    private readonly ApiFixture _fixture = fixture;

    public static TheoryData<string?, string?, HttpStatusCode> Cases =>
    new TheoryData<string?, string?, HttpStatusCode>
    {
        {DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), HttpStatusCode.OK},
        {DateTime.Now.AddMonths(-13).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), HttpStatusCode.Unauthorized}, //Unauthorized for over 365 days old queries
        {"", null, HttpStatusCode.BadRequest},
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task LongestDownwardTrend(string? fromDate, string? toDate, HttpStatusCode status)
    {
        var ct = TestContext.Current.CancellationToken;
        var result = await _fixture.Client.GetAsync($"{BaseUrl}/longestdownwardtrend?fromDate={fromDate}&toDate={toDate}", cancellationToken: ct);
        result.StatusCode.ShouldBeOneOf(status, HttpStatusCode.TooManyRequests);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var data = await result.Content.ReadFromJsonAsync<LongestDownwardTrendResponse>(cancellationToken: ct);
            data.ShouldNotBeNull();
        }
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task HighestTradingVolume(string? fromDate, string? toDate, HttpStatusCode status)
    {
        var ct = TestContext.Current.CancellationToken;
        var result = await _fixture.Client.GetAsync($"{BaseUrl}/highestradingvolume?fromDate={fromDate}&toDate={toDate}", cancellationToken: ct);
        result.StatusCode.ShouldBeOneOf(status, HttpStatusCode.TooManyRequests);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var data = await result.Content.ReadFromJsonAsync<HighestTradingVolumeResponse>(cancellationToken: ct);
            data.ShouldNotBeNull();
        }
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public async Task BuyAndSell(string? fromDate, string? toDate, HttpStatusCode status)
    {
        var ct = TestContext.Current.CancellationToken;
        var result = await _fixture.Client.GetAsync($"{BaseUrl}/buyandsell?fromDate={fromDate}&toDate={toDate}", cancellationToken: ct);
        result.StatusCode.ShouldBeOneOf(status, HttpStatusCode.TooManyRequests);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            var data = await result.Content.ReadFromJsonAsync<BuyAndSellResponse>(cancellationToken: ct);
            data.ShouldNotBeNull();
        }
    }

    [Theory]
    [InlineData("/swagger")]
    [InlineData("/scalar")]
    public async Task DocsUI(string endpoint)
    {
        var ct = TestContext.Current.CancellationToken;
        var result = await _fixture.Client.GetAsync(endpoint, cancellationToken: ct);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("/swagger/v1/swagger.json")]
    [InlineData("/openapi/v1.json")]
    public async Task DocsJson(string endpoint)
    {
        var ct = TestContext.Current.CancellationToken;
        var result = await _fixture.Client.GetAsync(endpoint, cancellationToken: ct);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var data = await result.Content.ReadAsStringAsync(cancellationToken: ct);

        Should.NotThrow(() =>
        {
            using var _ = JsonDocument.Parse(data);
        });
    }

    [Fact]
    public async Task Health()
    {
        var ct = TestContext.Current.CancellationToken;
        var result = await _fixture.Client.GetAsync("/health", cancellationToken: ct);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
