using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

public static class Services
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddSerilog();

        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();

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
        services.Configure<KestrelServerOptions>(opt =>
        {
            opt.AddServerHeader = false;
        });

        services.AddOutputCache(opt =>
        {
            opt.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
            opt.AddBasePolicy(builder => builder.Cache());
        });

        services.AddHsts(opt =>
        {
            opt.MaxAge = TimeSpan.FromDays(365);
            opt.IncludeSubDomains = true;
            opt.Preload = true;
        });

        services.AddHealthChecks();

        services.AddHttpClient();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
        });

        services.AddScoped<IMarketClient, MarketClient>();
        services.AddScoped<IMarketService, MarketService>();
    }
}