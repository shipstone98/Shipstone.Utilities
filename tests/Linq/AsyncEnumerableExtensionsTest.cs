using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Shipstone.Utilities.Linq;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Collections;

public sealed class AsyncEnumerableExtensionsTest
{
#region ToListAsync method
    [Fact]
    public async Task TestToListAsync_Invalid()
    {
        // Act
        ArgumentException ex =
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                AsyncEnumerableExtensions.ToListAsync<Object>(null!));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

    [Fact]
    public async Task TestToListAsync_Valid_Empty()
    {
        // Arrange
        MockAsyncEnumerable<Object> source = new();

        source._getAsyncEnumeratorFunc = () =>
        {
            MockAsyncEnumerator<Object> enumerator = new();
            enumerator._disposeAction = () => { };
            enumerator._moveNextFunc = () => false;
            return enumerator;
        };

        // Act
        List<Object> result =
            await AsyncEnumerableExtensions.ToListAsync(source);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task TestToListAsync_Valid_NotEmpty()
    {
        // Arrange
        const int COUNT = 5;
        IReadOnlyList<int> sourceList = new int[COUNT] { 1, 2, 3, 4, 5 };
        MockAsyncEnumerable<int> source = new();

        source._getAsyncEnumeratorFunc = () =>
        {
            IEnumerator<int> sourceEnumerator = sourceList.GetEnumerator();
            MockAsyncEnumerator<int> enumerator = new();
            enumerator._disposeAction = sourceEnumerator.Dispose;
            enumerator._moveNextFunc = sourceEnumerator.MoveNext;
            enumerator._currentFunc = () => sourceEnumerator.Current;
            return enumerator;
        };

        // Act
        List<int> result = await AsyncEnumerableExtensions.ToListAsync(source);

        // Assert
        Assert.Equal(COUNT, result.Count);

        for (int i = 0; i < COUNT; i ++)
        {
            Assert.Equal(sourceList[i], result[i]);
        }
    }
#endregion

#region ToSortedSetAsync method
    [Fact]
    public async Task TestToSortedSetAsync_Invalid()
    {
        // Act
        ArgumentException ex =
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                AsyncEnumerableExtensions.ToSortedSetAsync<Object>(null!));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

#region Valid arguments
    [Fact]
    public async Task TestToSortedSetAsync_Valid_Empty()
    {
        // Arrange
        MockAsyncEnumerable<Object> source = new();

        source._getAsyncEnumeratorFunc = () =>
        {
            MockAsyncEnumerator<Object> enumerator = new();
            enumerator._disposeAction = () => { };
            enumerator._moveNextFunc = () => false;
            return enumerator;
        };

        // Act
        SortedSet<Object> result =
            await AsyncEnumerableExtensions.ToSortedSetAsync(source);

        // Assert
        Assert.NotNull(result.Comparer);
        Assert.Empty(result);
        Assert.Null(result.Max);
        Assert.Null(result.Min);
    }

    [Fact]
    public async Task TestToSortedSetAsync_Valid_NotEmpty_ComparerNotNull()
    {
        // Arrange
        const int COUNT = 5;
        IEnumerable<int> sourceCollection = new int[COUNT] { 1, 2, 3, 4, 5 };
        MockAsyncEnumerable<int> source = new();
        IComparer<int> comparer = new MockInt32Comparer();

        source._getAsyncEnumeratorFunc = () =>
        {
            IEnumerator<int> sourceEnumerator =
                sourceCollection.GetEnumerator();

            MockAsyncEnumerator<int> enumerator = new();
            enumerator._disposeAction = sourceEnumerator.Dispose;
            enumerator._moveNextFunc = sourceEnumerator.MoveNext;
            enumerator._currentFunc = () => sourceEnumerator.Current;
            return enumerator;
        };

        // Act
        SortedSet<int> result =
            await AsyncEnumerableExtensions.ToSortedSetAsync(source, comparer);

        // Assert
        Assert.Same(comparer, result.Comparer);
        Assert.Equal(COUNT, result.Count);
        Assert.Equal(1, result.Max);
        Assert.Equal(COUNT, result.Min);
        Assert.True(sourceCollection.Reverse().SequenceEqual(result));
    }

    [Fact]
    public async Task TestToSortedSetAsync_Valid_NotEmpty_ComparerNull()
    {
        // Arrange
        const int COUNT = 5;
        IEnumerable<int> sourceCollection = new int[COUNT] { 1, 2, 3, 4, 5 };
        MockAsyncEnumerable<int> source = new();

        source._getAsyncEnumeratorFunc = () =>
        {
            IEnumerator<int> sourceEnumerator =
                sourceCollection.GetEnumerator();

            MockAsyncEnumerator<int> enumerator = new();
            enumerator._disposeAction = sourceEnumerator.Dispose;
            enumerator._moveNextFunc = sourceEnumerator.MoveNext;
            enumerator._currentFunc = () => sourceEnumerator.Current;
            return enumerator;
        };

        // Act
        SortedSet<int> result =
            await AsyncEnumerableExtensions.ToSortedSetAsync(source);

        // Assert
        Assert.NotNull(result.Comparer);
        Assert.Equal(COUNT, result.Count);
        Assert.Equal(COUNT, result.Max);
        Assert.Equal(1, result.Min);
        Assert.True(sourceCollection.SequenceEqual(result));
    }
#endregion
#endregion

#region WhereNotNullAsync method
    [Fact]
    public void TestWhereNotNullAsync_Invalid()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                AsyncEnumerableExtensions.WhereNotNullAsync<Object>(null!));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

    [Fact]
    public async Task TestWhereNotNullAsync_Valid_Empty()
    {
        // Arrange
        MockAsyncEnumerable<Object?> source = new();

        source._getAsyncEnumeratorFunc = () =>
        {
            MockAsyncEnumerator<Object?> enumerator = new();
            enumerator._disposeAction = () => { };
            enumerator._moveNextFunc = () => false;
            return enumerator;
        };

        // Act
        IAsyncEnumerable<Object> result =
            AsyncEnumerableExtensions.WhereNotNullAsync(source);

        // Assert
        IAsyncEnumerator<Object> enumerator = result.GetAsyncEnumerator();
        Assert.False(await enumerator.MoveNextAsync());
    }

    [Fact]
    public async Task TestWhereNotNullAsync_Valid_NotEmpty()
    {
        // Arrange
        const int INTEGER = 12345;
        const String STRING = "Hello, world!";

        IEnumerable<Object?> sourceCollection =
            new Object?[] { null, INTEGER, STRING };

        MockAsyncEnumerable<Object?> source = new();

        source._getAsyncEnumeratorFunc = () =>
        {
            IEnumerator<Object?> sourceEnumerator =
                sourceCollection.GetEnumerator();

            MockAsyncEnumerator<Object?> enumerator = new();
            enumerator._disposeAction = sourceEnumerator.Dispose;
            enumerator._moveNextFunc = sourceEnumerator.MoveNext;
            enumerator._currentFunc = () => sourceEnumerator.Current;
            return enumerator;
        };

        // Act
        IAsyncEnumerable<Object> result =
            AsyncEnumerableExtensions.WhereNotNullAsync(source);

        // Assert
        ICollection<Object> remaining = new List<Object> { INTEGER, STRING };

        await foreach (Object item in result)
        {
            Assert.True(remaining.Remove(item));
        }

        Assert.Empty(remaining);
    }
#endregion
}
