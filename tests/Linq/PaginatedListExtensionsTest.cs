using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using Shipstone.Utilities.Collections;
using Shipstone.Utilities.Linq;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Linq;

public sealed class PaginatedListExtensionsTest
{
#region Select method
    [Fact]
    public void TestSelect_Invalid_SelectorNull()
    {
        // Arrange
        IReadOnlyPaginatedList<Object> source =
            new MockReadOnlyPaginatedList<Object>();

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                PaginatedListExtensions.Select<Object, Object>(
                    source,
                    null!
                ));

        // Assert
        Assert.Equal("selector", ex.ParamName);
    }

    [Fact]
    public void TestSelect_Invalid_SourceNull()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                PaginatedListExtensions.Select<Object, Object>(
                    null!,
                    (_, _) => throw new NotImplementedException()
                ));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

    [Fact]
    public void TestSelect_Valid_Empty()
    {
        // Arrange
        IEnumerable<Object> sourceCollection = Array.Empty<Object>();
        MockReadOnlyPaginatedList<Object> source = new();
        source._getEnumeratorFunc = sourceCollection.GetEnumerator;
        source._totalCountFunc = () => 0;
        source._pageIndexFunc = () => 0;
        source._pageCountFunc = () => 1;

        // Act
        IReadOnlyPaginatedList<Object> result =
            PaginatedListExtensions.Select<Object, Object>(
                source,
                (_, _) => throw new NotImplementedException()
            );

        // Assert
        result.AssertEmpty();
    }

    [Fact]
    public void TestSelect_Valid_NotEmpty()
    {
        // Arrange
        const int COUNT = 5;

        IEnumerable<int> sourceCollection =
            new int[COUNT] { 11, 12, 13, 14, 15 };

        const int TOTAL_COUNT = 113;
        const int PAGE_INDEX = 17;
        const int PAGE_COUNT = 23;
        MockReadOnlyPaginatedList<int> source = new();
        source._getEnumeratorFunc = sourceCollection.GetEnumerator;
        source._totalCountFunc = () => TOTAL_COUNT;
        source._pageIndexFunc = () => PAGE_INDEX;
        source._pageCountFunc = () => PAGE_COUNT;
        ISet<int> indices = new SortedSet<int>();

        // Act
        IReadOnlyPaginatedList<String> result =
            PaginatedListExtensions.Select(
                source,
                (n, i) =>
                {
                    indices.Add(i);
                    return n.ToString();
                });

        // Assert
        IReadOnlyList<String> list =
            sourceCollection
                .Select(n => n.ToString())
                .ToList();

        result.AssertEqual(list, TOTAL_COUNT, PAGE_INDEX, PAGE_COUNT);
        Assert.Equal(COUNT, indices.Count);
        int index = 0;

        foreach (int indicesIndex in indices)
        {
            Assert.Equal(index ++, indicesIndex);
        }
    }
#endregion
}
