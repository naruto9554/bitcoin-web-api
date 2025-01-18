using System.Globalization;
using Asp.Versioning;
using Microsoft.OpenApi.Any;
using Services;
using System;

namespace Api.Setup;

internal static class ApiEndpoints
{
    private static readonly OpenApiString ExampleFromDate = new(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    private static readonly OpenApiString ExampleToDate = new(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

    public static void ConfigureEndpoints(this WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        var group = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        ConfigureEndpoint(group, "/longestdownwardtrend",
            "Get longest downward trend in days between given dates",
            async (IMarketService service, DateOnly fromDate, DateOnly toDate) =>
            {
                try
                {
                    var result = await service.GetLongestDownwardTrend(fromDate, toDate);
                    if (result is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(new
                    {
                        Days = result
                    });
                }
                catch (HttpRequestException ex)
                {
                    return Results.Problem(statusCode: (int?)ex.StatusCode);
                }
            });

        ConfigureEndpoint(group, "/highestradingvolume",
            "Get the date with the highest trading volume between given dates",
            async (IMarketService service, DateOnly fromDate, DateOnly toDate) =>
            {
                try
                {
                    var result = await service.GetHighestTradingVolume(fromDate, toDate);
                    if (result is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(new
                    {
                        result.Value.Date,
                        result.Value.Volume,
                    });
                }
                catch (HttpRequestException ex)
                {
                    return Results.Problem(statusCode: (int?)ex.StatusCode);
                }
            });

        ConfigureEndpoint(group, "/buyandsell",
            "Get pair of dates when it is best to buy and sell between given dates",
            async (IMarketService service, DateOnly fromDate, DateOnly toDate) =>
            {
                try
                {
                    var result = await service.GetBestBuyAndSellDates(fromDate, toDate);
                    if (result is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(new
                    {
                        result.Value.SellDate,
                        result.Value.BuyDate,
                    });
                }
                catch (HttpRequestException ex)
                {
                    return Results.Problem(statusCode: (int?)ex.StatusCode);
                }
            });
    }

    private static void ConfigureEndpoint(RouteGroupBuilder group, string route, string summary, Delegate handler)
    {
        group.MapGet(route, handler)
            .WithOpenApi(operation =>
            {
                operation.Summary = summary;
                operation.Parameters[0].Example = ExampleFromDate;
                operation.Parameters[^1].Example = ExampleToDate;
                return operation;
            });
    }
}
