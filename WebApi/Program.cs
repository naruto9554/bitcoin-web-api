using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IMarketStore, MarketStore>();
builder.Services.AddScoped<IMarketService, MarketService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var serializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    WriteIndented = true,
};


app.MapGet("/longestdownwardtrend", async (string fromDate, string toDate) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<IMarketService>();
        return await service.GetLongestDownwardTrend(fromDate, toDate);
    }
}).WithName("GetLongestDownwardTrend");

app.MapGet("/highestradingvolume", async (string fromDate, string toDate) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<IMarketService>();
        var result = await service.GetHighestTradingVolume(fromDate, toDate);
        return JsonSerializer.Serialize<TradeVolume>(result, serializerOptions);
    }
}).WithName("GetHighestTradingVolumeDate");

app.MapGet("/buyandsell", async (string fromDate, string toDate) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<IMarketService>();
        var result = await service.GetBestBuyAndSellDates(fromDate, toDate);
        return JsonSerializer.Serialize<TradeDate>(result, serializerOptions);
    }
}).WithName("GetBestBuyAndSellDates");

app.Run();