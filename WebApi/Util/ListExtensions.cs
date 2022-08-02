using System.Collections.Generic;

public static class ListExtensions
{
    public static int LongestConsecutiveDecreasingSubset(this List<decimal> values)
    {
        var longest = 0;
        var counter = 0;
        for (var i = 1; i < values.Count; i++)
        {
            if (values[i] < values[i - 1])
            {
                counter++;
                if (counter > longest)
                {
                    longest = counter;
                }
            }
            else
            {
                counter = 0;
            }
        }

        return longest;
    }

    public static bool IsOnlyDecreasing(this List<decimal> values)
    {
        for (var i = 1; i < values.Count; i++)
        {
            if (values[i] > values[i - 1])
            {
                return false;
            }
        }

        return true;
    }
}