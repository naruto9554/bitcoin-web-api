using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

public static class Services
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.Configure<JsonOptions>(opt =>
        {
            var serializerOptions = opt.SerializerOptions;
            serializerOptions.WriteIndented = true;
            serializerOptions.IncludeFields = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            serializerOptions.PropertyNameCaseInsensitive = true;
            serializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.Configure<KestrelServerOptions>(opt => { opt.AddServerHeader = false; });

        services.AddOutputCache(opt =>
        {
            opt.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
            opt.AddBasePolicy(builder => builder.Cache());
        });

        services.AddHttpClient();

        services.AddScoped<IMarketClient, MarketClient>();
        services.AddScoped<IMarketService, MarketService>();
    }
}