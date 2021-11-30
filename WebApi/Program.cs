using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Options
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(opt =>
{
    var serializerOptions = opt.SerializerOptions;
    serializerOptions.WriteIndented = true;
    serializerOptions.IncludeFields = true;
    serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    serializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    serializerOptions.PropertyNameCaseInsensitive = true;
});

// Services
builder.Services.AddHttpClient();
builder.Services.AddScoped<IMarketStore, MarketStore>();
builder.Services.AddScoped<IMarketService, MarketService>();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.EnvironmentName.ToLower().Contains("https"))
{
    app.UseHttpsRedirection();
}

// Routes
app.MapGet("/longestdownwardtrend", async (IMarketService service, string fromDate, string toDate) =>
{
    var result = await service.GetLongestDownwardTrend(fromDate, toDate);
    if (result == null) return Results.NotFound();
    return Results.Ok(result);
});

app.MapGet("/highestradingvolume", async (IMarketService service, string fromDate, string toDate) =>
{
    var result = await service.GetHighestTradingVolume(fromDate, toDate);
    if (result == null) return Results.NotFound();
    return Results.Ok(result);
});


app.MapGet("/buyandsell", async (IMarketService service, string fromDate, string toDate) =>
{
    var result = await service.GetBestBuyAndSellDates(fromDate, toDate);
    if (result == null) return Results.NotFound();
    return Results.Ok(result);
});

app.Run();