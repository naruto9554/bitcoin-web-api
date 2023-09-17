using System;

public static class DateTimeOffsetExtensions
{
    public static DateOnly ToDateOnly(this DateTimeOffset offset)
    {
        return new DateOnly(offset.Year, offset.Month, offset.Day);
    }

}