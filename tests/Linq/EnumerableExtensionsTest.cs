using System;
using System.Collections.Generic;
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
}
