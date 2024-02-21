using System.Collections.Generic;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this IList<T>? list)
    {
        return list is null || list.Count == 0;
    }

    public static int LongestConsecutiveDecreasingSubset<T>(this IList<T>? list, IComparer<T>? comparer = null)
    {
        comparer ??= Comparer<T>.Default;

        var longest = 0;
        var counter = 0;
        for (var i = 1; i < list?.Count; i++)
        {
            if (comparer.Compare(list[i - 1], list[i]) > 0)
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

    public static bool IsOrderedDecreasing<T>(this IList<T>? list, IComparer<T>? comparer = null)
    {
        comparer ??= Comparer<T>.Default;

        if (list?.Count > 1)
        {
            for (var i = 1; i < list.Count; i++)
            {
                if (comparer.Compare(list[i - 1], list[i]) < 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
}