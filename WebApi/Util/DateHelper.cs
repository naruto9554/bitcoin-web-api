using System;
using System.Globalization;

public static class DateHelper
{
    public static long DateToUnixTime(string datetime)
    {
        var dtOffset = DateTimeOffset.Parse(datetime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        return dtOffset.ToUnixTimeSeconds();
    }
}