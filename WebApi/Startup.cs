using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class Startup
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
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
        });
        services.Configure<KestrelServerOptions>(opt => { opt.AddServerHeader = false; });

        services.AddHttpClient();
        services.AddScoped<IMarketStore, MarketStore>();
        services.AddScoped<IMarketService, MarketService>();

        return services;
    }

    public static IApplicationBuilder ConfigureMiddleware(this IApplicationBuilder builder, IWebHostEnvironment env)
    {
        builder.UseSwagger();
        builder.UseSwaggerUI();

        if (env.IsProduction())
        {
            builder.UseHttpsRedirection();
        }

        return builder;
    }
}