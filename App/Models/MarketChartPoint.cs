using System;

public class MarketChartPoint
{
    public DateTimeOffset Date { get; set; }
    public decimal Price { get; set; }
    public decimal MarketCap { get; set; }
    public decimal TotalVolume { get; set; }
}