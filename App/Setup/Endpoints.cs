using System;
using System.Linq;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;

public static class Endpoints
{
    private static readonly OpenApiString ExampleFromDate = new("2022-01-01");
    private static readonly OpenApiString ExampleToDate = new("2022-01-31");

    public static void ConfigureEndpoints(this WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .ReportApiVersions()
        .Build();

        var group = app
        .MapGroup("api/v{version:apiVersion}")
        .WithApiVersionSet(apiVersionSet);

        group.MapGet("/longestdownwardtrend", async (IMarketService service, DateOnly fromDate, DateOnly toDate) =>
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
            catch (System.Net.Http.HttpRequestException ex)
            {
                return Results.Problem(statusCode: (int?)ex.StatusCode);
            }
            catch (Exception)
            {
                return Results.Problem();
            }
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get longest downward trend in days between given dates";
            operation.Parameters.First().Example = ExampleFromDate;
            operation.Parameters.Last().Example = ExampleToDate;
            return operation;
        });

        group.MapGet("/highestradingvolume", async (IMarketService service, DateOnly fromDate, DateOnly toDate) =>
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
                    Date = result?.Date,
                    Volume = result?.Volume,
                });
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                return Results.Problem(statusCode: (int?)ex.StatusCode);
            }
            catch (Exception)
            {
                return Results.Problem();
            }
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get the date with the highest trading volume between given dates";
            operation.Parameters.First().Example = ExampleFromDate;
            operation.Parameters.Last().Example = ExampleToDate;
            return operation;
        });

        group.MapGet("/buyandsell", async (IMarketService service, DateOnly fromDate, DateOnly toDate) =>
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
                    SellDate = result?.SellDate,
                    BuyDate = result?.BuyDate,
                });
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                return Results.Problem(statusCode: (int?)ex.StatusCode);
            }
            catch (Exception)
            {
                return Results.Problem();
            }
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get pair of dates when it is best to buy and sell between given dates";
            operation.Parameters.First().Example = ExampleFromDate;
            operation.Parameters.Last().Example = ExampleToDate;
            return operation;
        });
    }
}