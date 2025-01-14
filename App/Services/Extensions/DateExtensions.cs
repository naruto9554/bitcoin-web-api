namespace Services.Extensions;

public static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTimeOffset offset)
    {
        return new DateOnly(offset.Year, offset.Month, offset.Day);
    }

    public static DateTimeOffset FromDateOnly(this DateOnly date)
    {
        var time = new TimeOnly(0, 0);
        var datetime = date.ToDateTime(time);
        return new DateTimeOffset(datetime, TimeSpan.Zero);
    }

    public static long ToUnixTimestamp(this DateOnly date)
    {
        var dateTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        var offset = new DateTimeOffset(dateTime, TimeSpan.Zero);
        return offset.ToUnixTimeSeconds();
    }

    public static DateOnly FromUnixTimestamp(this long unixTimestamp)
    {
        var offset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        return new DateOnly(offset.Year, offset.Month, offset.Day);
    }
}