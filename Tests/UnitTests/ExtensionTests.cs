using Services.Extensions;
using Shouldly;
using Xunit;

namespace UnitTests;

public class ExtensionTests
{
    [Theory]
    [InlineData(2024, 8, 11)]
    [InlineData(1970, 1, 1)]
    public void DateTimeOffsetToDateOnly(int year, int month, int day)
    {
        var dateTimeOffset = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
        var dateOnly = dateTimeOffset.ToDateOnly();

        dateOnly.Year.ShouldBe(year);
        dateOnly.Month.ShouldBe(month);
        dateOnly.Day.ShouldBe(day);
    }

    [Theory]
    [InlineData(2024, 8, 11)]
    [InlineData(1970, 1, 1)]
    public void DateTimeOffsetFromDateOnly(int year, int month, int day)
    {
        var dateOnly = new DateOnly(year, month, day);
        var dateTimeOffset = dateOnly.FromDateOnly();

        dateTimeOffset.Year.ShouldBe(year);
        dateTimeOffset.Month.ShouldBe(month);
        dateTimeOffset.Day.ShouldBe(day);
        dateTimeOffset.Offset.Hours.ShouldBe(0);
    }

    [Theory]
    [InlineData(2024, 8, 11, 1723334400)]
    [InlineData(1970, 1, 1, 0)]
    public void DateOnlyToUnixTimestamp(int year, int month, int day, long expectedTimestamp)
    {
        var dateOnly = new DateOnly(year, month, day);
        var unixTimestamp = dateOnly.ToUnixTimestamp();

        unixTimestamp.ShouldBe(expectedTimestamp);
    }

    [Theory]
    [InlineData(1723334400, 2024, 8, 11)]
    [InlineData(0, 1970, 1, 1)]
    public void DateOnlyFromUnixTimestamp(long unixTimestamp, int expectedYear, int expectedMonth, int expectedDay)
    {
        var dateOnly = unixTimestamp.FromUnixTimestamp();

        dateOnly.Year.ShouldBe(expectedYear);
        dateOnly.Month.ShouldBe(expectedMonth);
        dateOnly.Day.ShouldBe(expectedDay);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[] { }, true)]
    [InlineData(new int[] { 1 }, false)]
    [InlineData(new int[] { 1, 2 }, false)]
    public void IsNullOrEmpty(int[]? input, bool expected)
    {
        var result = input.IsNullOrEmpty();

        result.ShouldBe(expected);
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

        result.ShouldBe(expected);
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

        result.ShouldBe(expected);
    }
}
