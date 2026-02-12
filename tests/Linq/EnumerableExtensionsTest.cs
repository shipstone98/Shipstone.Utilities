using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Shipstone.Utilities.Linq;

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
                    null!
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
                    (_, _, _) => throw new NotImplementedException()
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
                (_, _, _) => throw new NotImplementedException()
            );

        // Assert
        IAsyncEnumerator<Object> enumerator = result.GetAsyncEnumerator();
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
                }
            );

        // Assert
        IAsyncEnumerator<String> enumerator = result.GetAsyncEnumerator();

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
                    null!
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
                    (_, _, _) => throw new NotImplementedException()
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
                (_, _, _) => throw new NotImplementedException()
            );

        // Assert
        await using IAsyncEnumerator<Object> enumerator =
            resultActual.GetAsyncEnumerator();

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
                }
            );

        // Assert
        IEnumerable<char> resultExpected =
            new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        using IEnumerator<char> expectedEnumerator =
            resultExpected.GetEnumerator();

        await using IAsyncEnumerator<char> actualEnumerator =
            resultActual.GetAsyncEnumerator();

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
