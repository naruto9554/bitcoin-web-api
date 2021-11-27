using System;
using System.Globalization;

public static class DateHelper
{
    public static long DateToUnixTime(string date)
    {
        var dtOffset = DateTimeOffset.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
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