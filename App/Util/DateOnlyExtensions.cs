using System;

public static class DateOnlyExtensions
{
    public static long ToUnixTimestamp(this DateOnly date)
    {
        var offset = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, TimeSpan.Zero);
        return offset.ToUnixTimeSeconds();
    }

    public static DateOnly FromUnixTimestamp(this long unixTimestamp)
    {
        var offset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        return new DateOnly(offset.Year, offset.Month, offset.Day);
    }
}