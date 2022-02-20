using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/longestdownwardtrend", async (IMarketService service, string fromDate, string toDate) =>
        {
            var result = await service.GetLongestDownwardTrend(fromDate, toDate);
            if (result is null) return Results.NotFound();
            return Results.Ok(result);
        });

        endpoints.MapGet("/highestradingvolume", async (IMarketService service, string fromDate, string toDate) =>
        {
            var result = await service.GetHighestTradingVolume(fromDate, toDate);
            if (result is null) return Results.NotFound();
            return Results.Ok(result);
        });


        endpoints.MapGet("/buyandsell", async (IMarketService service, string fromDate, string toDate) =>
        {
            var result = await service.GetBestBuyAndSellDates(fromDate, toDate);
            if (result is null) return Results.NotFound();
            return Results.Ok(result);
        });

        return endpoints;
    }
}