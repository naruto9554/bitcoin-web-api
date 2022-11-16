using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;

public static class Endpoints
{
    private static readonly OpenApiString ExampleFromDate = new OpenApiString("2022-01-01");
    private static readonly OpenApiString ExampleToDate = new OpenApiString("2022-01-31");

    public static IEndpointRouteBuilder ConfigureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/longestdownwardtrend", async (IMarketService service, string fromDate, string toDate) =>
        {
            var result = await service.GetLongestDownwardTrend(fromDate, toDate);
            if (result is null) return Results.NotFound();
            return Results.Ok(new
            {
                Days = result
            });
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get longest downward trend in days between given dates";
            operation.Parameters.First().Example = ExampleFromDate;
            operation.Parameters.Last().Example = ExampleToDate;
            return operation;
        });

        endpoints.MapGet("/highestradingvolume", async (IMarketService service, string fromDate, string toDate) =>
        {
            var result = await service.GetHighestTradingVolume(fromDate, toDate);
            if (result is null) return Results.NotFound();
            return Results.Ok(new
            {
                Date = result?.Date,
                Volume = result?.Volume,
            });
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get the date with the highest trading volume between given dates";
            operation.Parameters.First().Example = ExampleFromDate;
            operation.Parameters.Last().Example = ExampleToDate;
            return operation;
        });

        endpoints.MapGet("/buyandsell", async (IMarketService service, string fromDate, string toDate) =>
        {
            var result = await service.GetBestBuyAndSellDates(fromDate, toDate);
            if (result is null) return Results.NotFound();
            return Results.Ok(new
            {
                SellDate = result?.SellDate,
                BuyDate = result?.BuyDate,
            });
        })
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get pair of dates when it is best to buy and sell between given dates";
            operation.Parameters.First().Example = ExampleFromDate;
            operation.Parameters.Last().Example = ExampleToDate;
            return operation;
        });

        return endpoints;
    }
}