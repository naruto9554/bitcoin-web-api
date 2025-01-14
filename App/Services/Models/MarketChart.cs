using System.Text.Json.Serialization;

namespace Services.Models;

public record MarketChart
{
    [JsonPropertyName("prices")]
    public decimal[][]? Prices { get; init; }
    [JsonPropertyName("market_caps")]
    public decimal[][]? MarketCaps { get; init; }
    [JsonPropertyName("total_volumes")]
    public decimal[][]? TotalVolumes { get; init; }
}