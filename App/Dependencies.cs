using Microsoft.Extensions.DependencyInjection;

public static class Dependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IMarketClient, MarketClient>();
        services.AddScoped<IMarketService, MarketService>();
    }
}