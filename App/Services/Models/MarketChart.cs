namespace Services.Models;

public record MarketChart
{
    public decimal[][]? Prices { get; init; }
    public decimal[][]? Market_caps { get; init; }
    public decimal[][]? Total_volumes { get; init; }
}