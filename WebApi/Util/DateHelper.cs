using System;
using System.Globalization;

public static class DateHelper
{
    public static long DateToUnixTime(string datetime)
    {
        var dtOffset = DateTimeOffset.Parse(datetime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        return dtOffset.ToUnixTimeSeconds();
    }

    public static DateTimeOffset UnixTimeToDateTimeOffset(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
    }

    public static string DateTimeOffsetToDate(DateTimeOffset datetime)
    {
        var format = "yyyy-MM-dd";
        return datetime.ToString(format);
    }
}