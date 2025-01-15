using System.Text.Json.Serialization;

namespace Services.Models;

public record MarketChart
{
    [JsonPropertyName("prices")]
    public IReadOnlyList<IReadOnlyList<decimal>>? Prices { get; init; }
    [JsonPropertyName("market_caps")] public IReadOnlyList<IReadOnlyList<decimal>>? MarketCaps { get; init; }
    [JsonPropertyName("total_volumes")]
    public IReadOnlyList<IReadOnlyList<decimal>>? TotalVolumes { get; init; }
}
