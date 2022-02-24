using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class Endpoints
{
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
        });

        return endpoints;
    }
}