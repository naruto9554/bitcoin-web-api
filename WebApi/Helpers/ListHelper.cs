using System.Collections.Generic;

public static class ListHelper
{
    public static int LongestConsecutiveDecreasingSubset(List<decimal> values)
    {
        var longest = 1;
        var counter = 0;
        for (var i = 1; i < values.Count; i++)
        {
            if (values[i] < values[i - 1])
            {
                counter++;
            }
            else
            {
                if (counter > longest)
                {
                    longest = counter;
                }
                counter = 0;
            }
        }

        return longest;
    }
}