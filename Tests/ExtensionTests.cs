using Xunit;

public class ExtensionTests
{
    [Theory]
    [InlineData(2024, 8, 11)]
    [InlineData(1970, 1, 1)]
    public void DateTimeOffsetToDateOnly(int year, int month, int day)
    {
        var dateTimeOffset = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        var dateOnly = dateTimeOffset.ToDateOnly();

        Assert.Equal(year, dateOnly.Year);
        Assert.Equal(month, dateOnly.Month);
        Assert.Equal(day, dateOnly.Day);
    }

    [Theory]
    [InlineData(2024, 8, 11)]
    [InlineData(1970, 1, 1)]
    public void DateTimeOffsetFromDateOnly(int year, int month, int day)
    {
        var dateOnly = new DateOnly(year, month, day);
        var dateTimeOffset = dateOnly.FromDateOnly();

        Assert.Equal(year, dateTimeOffset.Year);
        Assert.Equal(month, dateTimeOffset.Month);
        Assert.Equal(day, dateTimeOffset.Day);
        Assert.Equal(0, dateTimeOffset.Offset.Hours);
    }

    [Theory]
    [InlineData(2024, 8, 11, 1723334400)]
    [InlineData(1970, 1, 1, 0)]
    public void DateOnlyToUnixTimestamp(int year, int month, int day, long expectedTimestamp)
    {
        var dateOnly = new DateOnly(year, month, day);
        var unixTimestamp = dateOnly.ToUnixTimestamp();

        Assert.Equal(expectedTimestamp, unixTimestamp);
    }

    [Theory]
    [InlineData(1723334400, 2024, 8, 11)]
    [InlineData(0, 1970, 1, 1)]
    public void DateOnlyFromUnixTimestamp(long unixTimestamp, int expectedYear, int expectedMonth, int expectedDay)
    {
        var dateOnly = unixTimestamp.FromUnixTimestamp();

        Assert.Equal(expectedYear, dateOnly.Year);
        Assert.Equal(expectedMonth, dateOnly.Month);
        Assert.Equal(expectedDay, dateOnly.Day);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[] { }, true)]
    [InlineData(new int[] { 1 }, false)]
    [InlineData(new int[] { 1, 2 }, false)]
    public void IsNullOrEmpty(int[]? input, bool expected)
    {
        var result = input.IsNullOrEmpty();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new int[] { 5, 4, 3, 2, 1 }, 4)]
    [InlineData(new int[] { 5, 6, 4, 3, 7, 1 }, 2)]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 0)]
    [InlineData(new int[] { 5, 4, 4, 3, 2 }, 2)]
    [InlineData(new int[] { }, 0)]
    [InlineData(null, 0)]
    public void LongestConsecutiveDecreasingSubset(int[]? input, int expected)
    {
        var result = input.LongestConsecutiveDecreasingSubset();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new int[] { 5, 4, 3, 2, 1 }, true)]
    [InlineData(new int[] { 5, 6, 4, 3, 2 }, false)]
    [InlineData(new int[] { 5, 4, 4, 3, 2 }, true)]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, false)]
    [InlineData(new int[] { }, true)]
    [InlineData(null, true)]
    public void IsOrderedDecreasing(int[]? input, bool expected)
    {
        var result = input.IsOrderedDecreasing();

        Assert.Equal(expected, result);
    }
}