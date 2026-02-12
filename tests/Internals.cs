using System;
using System.Collections.Generic;
using Xunit;

using Shipstone.Utilities.Collections;

namespace Shipstone.UtilitiesTest;

internal static class Internals
{
    internal static void AssertEmpty<T>(this IReadOnlyPaginatedList<T> list)
    {
        IReadOnlyList<T> source = Array.Empty<T>();
        Internals.AssertEqual(list, source, 0, 0, 1);
    }

    internal static void AssertEqual<T>(
        this IReadOnlyPaginatedList<T> list,
        IReadOnlyList<T> source,
        int totalCount,
        int pageIndex,
        int pageCount
    )
    {
        Assert.Equal(source.Count, list.Count);
        Assert.Equal(pageCount, list.PageCount);
        Assert.Equal(pageIndex, list.PageIndex);
        Assert.Equal(totalCount, list.TotalCount);

        for (int i = 0; i < source.Count; i ++)
        {
            Assert.Equal(source[i], list[i]);
        }
    }

    internal static int[] CreateInt32Array(int length, int start = 1)
    {
        int[] array = new int[length];

        for (int i = 0; i < length; i ++)
        {
            array[i] = i + start;
        }

        return array;
    }
}
