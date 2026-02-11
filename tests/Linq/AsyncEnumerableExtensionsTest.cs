using System;
using System.Collections.Generic;
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
}
