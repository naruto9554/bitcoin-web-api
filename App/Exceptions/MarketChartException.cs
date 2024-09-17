[Serializable]
public class MarketChartException : Exception
{
    public MarketChartException()
    {
    }

    public MarketChartException(string message)
        : base(message)
    {
    }

    public MarketChartException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}