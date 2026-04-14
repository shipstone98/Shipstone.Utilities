using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Shipstone.Utilities.Linq;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Linq;

public sealed class EnumerableExtensionsTest
{
#region SelectAsync method
    [Fact]
    public void TestSelectAsync_Invalid_SelectorNull()
    {
        // Arrange
        IEnumerable<Object> source = Array.Empty<Object>();

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensions.SelectAsync<Object, Object>(
                    source,
                    null!,
                    TestContext.Current.CancellationToken
                ));

        // Assert
        Assert.Equal("selector", ex.ParamName);
    }

    [Fact]
    public void TestSelectAsync_Invalid_SourceNull()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensions.SelectAsync<Object, Object>(
                    null!,
                    (_, _, _) => throw new NotImplementedException(),
                    TestContext.Current.CancellationToken
                ));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

    [Fact]
    public async Task TestSelectAsync_Valid_Empty()
    {
        // Arrange
        IEnumerable<Object> source = Array.Empty<Object>();

        // Act
        IAsyncEnumerable<Object> result =
            EnumerableExtensions.SelectAsync<Object, Object>(
                source,
                (_, _, _) => throw new NotImplementedException(),
                TestContext.Current.CancellationToken
            );

        // Assert
        IAsyncEnumerator<Object> enumerator =
            result.GetAsyncEnumerator(TestContext.Current.CancellationToken);

        Assert.False(await enumerator.MoveNextAsync());
    }

    [Fact]
    public async Task TestSelectAsync_Valid_NotEmpty()
    {
        // Arrange
        const int COUNT = 10;
        IReadOnlyList<int> source = Internals.CreateInt32Array(COUNT);
        ISet<int> indices = new SortedSet<int>();

        // Act
        IAsyncEnumerable<String> result =
            EnumerableExtensions.SelectAsync(
                source,
                (n, i, _) =>
                {
                    indices.Add(i);
                    String s = n.ToString();
                    return Task.FromResult(s);
                },
                TestContext.Current.CancellationToken
            );

        // Assert
        IAsyncEnumerator<String> enumerator =
            result.GetAsyncEnumerator(TestContext.Current.CancellationToken);

        for (int i = 0; i < COUNT; i ++)
        {
            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(source[i].ToString(), enumerator.Current);
        }

        Assert.False(await enumerator.MoveNextAsync());
        Assert.Equal(COUNT, indices.Count);
        int index = 0;

        foreach (int indicesIndex in indices)
        {
            Assert.Equal(index ++, indicesIndex);
        }
    }
#endregion

#region SelectManyAsync method
    [Fact]
    public void TestSelectManyAsync_Invalid_SelectorNull()
    {
        // Arrange
        IEnumerable<Object> source = Array.Empty<Object>();

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensions.SelectManyAsync<Object, Object>(
                    source,
                    null!,
                    TestContext.Current.CancellationToken
                ));

        // Assert
        Assert.Equal("selector", ex.ParamName);
    }

    [Fact]
    public void TestSelectManyAsync_Invalid_SourceNull()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensions.SelectManyAsync<Object, Object>(
                    null!,
                    (_, _, _) => throw new NotImplementedException(),
                    TestContext.Current.CancellationToken
                ));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

    [Fact]
    public async Task TestSelectManyAsync_Valid_Empty()
    {
        // Arrange
        IEnumerable<Object> source = Array.Empty<Object>();

        // Act
        IAsyncEnumerable<Object> resultActual =
            EnumerableExtensions.SelectManyAsync<Object, Object>(
                source,
                (_, _, _) => throw new NotImplementedException(),
                TestContext.Current.CancellationToken
            );

        // Assert
        await using IAsyncEnumerator<Object> enumerator =
            resultActual.GetAsyncEnumerator(TestContext.Current.CancellationToken);

        Assert.False(await enumerator.MoveNextAsync());
    }

    [Fact]
    public async Task TestSelectManyAsync_Valid_NotEmpty()
    {
        // Arrange
        IEnumerable<String> source = new String[] { "123", "456", "789" };
        ISet<int> indicesActual = new SortedSet<int>();

        // Act
        IAsyncEnumerable<char> resultActual =
            EnumerableExtensions.SelectManyAsync(
                source,
                (s, i, _) =>
                {
                    indicesActual.Add(i);
                    return EnumerableExtensionsTest.SelectAsync(s);
                },
                TestContext.Current.CancellationToken
            );

        // Assert
        IEnumerable<char> resultExpected =
            new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        using IEnumerator<char> expectedEnumerator =
            resultExpected.GetEnumerator();

        await using IAsyncEnumerator<char> actualEnumerator =
            resultActual.GetAsyncEnumerator(TestContext.Current.CancellationToken);

        while (expectedEnumerator.MoveNext())
        {
            Assert.True(await actualEnumerator.MoveNextAsync());
            Assert.Equal(expectedEnumerator.Current, actualEnumerator.Current);
        }

        Assert.False(await actualEnumerator.MoveNextAsync());
        IEnumerable<int> indicesExpected = new int[] { 0, 1, 2 };
        Assert.True(indicesExpected.SequenceEqual(indicesActual));
    }
#endregion

#region ToSortedSet method
    [Fact]
    public void TestToSortedSet_Invalid()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                EnumerableExtensions.ToSortedSet<Object>(null!));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

#region Valid arguments
    [Fact]
    public void TestToSortedSet_Valid_Empty()
    {
        // Arrange
        IEnumerable<Object> source = Array.Empty<Object>();

        // Act
        SortedSet<Object> result = EnumerableExtensions.ToSortedSet(source);

        // Assert
        Assert.NotNull(result.Comparer);
        Assert.Empty(result);
        Assert.Null(result.Max);
        Assert.Null(result.Min);
    }

    [Fact]
    public void TestToSortedSet_Valid_NotEmpty_ComparerNotNull()
    {
        // Arrange
        const int COUNT = 5;
        IEnumerable<int> source = new int[COUNT] { 1, 2, 3, 4, 5 };
        IComparer<int> comparer = new MockInt32Comparer();

        // Act
        SortedSet<int> result =
            EnumerableExtensions.ToSortedSet(source, comparer);

        // Assert
        Assert.Same(comparer, result.Comparer);
        Assert.Equal(COUNT, result.Count);
        Assert.Equal(1, result.Max);
        Assert.Equal(COUNT, result.Min);
        Assert.True(source.Reverse().SequenceEqual(result));
    }

    [Fact]
    public void TestToSortedSet_Valid_NotEmpty_ComparerNull()
    {
        // Arrange
        const int COUNT = 5;
        IEnumerable<int> source = new int[COUNT] { 1, 2, 3, 4, 5 };

        // Act
        SortedSet<int> result = EnumerableExtensions.ToSortedSet(source);

        // Assert
        Assert.NotNull(result.Comparer);
        Assert.Equal(COUNT, result.Count);
        Assert.Equal(COUNT, result.Max);
        Assert.Equal(1, result.Min);
        Assert.True(source.SequenceEqual(result));
    }
#endregion
#endregion

#pragma warning disable CS1998
    private static async IAsyncEnumerable<char> SelectAsync(String source)
#pragma warning restore CS1998
    {
        foreach (char c in source)
        {
            yield return c;
        }
    }
}
