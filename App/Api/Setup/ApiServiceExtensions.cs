using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Serilog;
using Services;

namespace Api.Setup;

internal static class ApiServiceExtensions
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
        services.ConfigureOptions<SwaggerOptions>();

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

        services.AddRateLimiter(opt =>
        {
            opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: httpContext.Request.Path.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 2
                });
            });

            opt.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken);
            };
        });

        services.AddScoped<IMarketClient, MarketClient>();
        services.AddScoped<IMarketService, MarketService>();
    }
}
