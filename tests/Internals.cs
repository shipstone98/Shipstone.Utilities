using System;
using System.Collections.Generic;
using Xunit;

using Shipstone.Utilities.Collections;

namespace Shipstone.UtilitiesTest;

internal static class Internals
{
    internal static void AssertEmpty<T>(this ReadOnlyPaginatedCollection<T> collection)
    {
        IReadOnlyList<T> list = Array.Empty<T>();
        Internals.AssertEqual(collection, list, 0, 0, 1);
    }

    internal static void AssertEqual<T>(
        this ReadOnlyPaginatedCollection<T> collection,
        IReadOnlyList<T> list,
        int totalCount,
        int pageIndex,
        int pageCount
    )
    {
        Assert.Equal(list.Count, collection.Count);
        Assert.Equal(pageCount, collection.PageCount);
        Assert.Equal(pageIndex, collection.PageIndex);
        Assert.Equal(totalCount, collection.TotalCount);

        for (int i = 0; i < list.Count; i ++)
        {
            Assert.Equal(list[i], collection[i]);
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
