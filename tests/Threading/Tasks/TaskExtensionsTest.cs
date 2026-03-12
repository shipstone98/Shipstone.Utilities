using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Shipstone.UtilitiesTest.Threading.Tasks;

public sealed class TaskExtensionsTest
{
#region AsAsyncEnumerable method
    [Fact]
    public void TestAsAsyncEnumerable_Invalid()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                Utilities.Threading.Tasks.TaskExtensions.AsAsyncEnumerable<Object>(null!));

        // Assert
        Assert.Equal("source", ex.ParamName);
    }

    [Fact]
    public async Task TestAsAsyncEnumerable_Valid_Empty()
    {
        // Arrange
        Task<int[]> source = Task.Run(Array.Empty<int>);

        // Act
        IAsyncEnumerable<int> result =
            Utilities.Threading.Tasks.TaskExtensions.AsAsyncEnumerable(source);

        // Assert
        await using IAsyncEnumerator<int> resultEnumerator =
            result.GetAsyncEnumerator();

        Assert.False(await resultEnumerator.MoveNextAsync());
    }

    [Fact]
    public async Task TestAsAsyncEnumerable_Valid_NotEmpty()
    {
        // Arrange
        int[] sourceCollection = new int[] { 1, 2, 3, 4, 5 };
        Task<int[]> source = Task.Run(() => sourceCollection);

        // Act
        IAsyncEnumerable<int> result =
            Utilities.Threading.Tasks.TaskExtensions.AsAsyncEnumerable(source);

        // Assert
        IEnumerator enumerator = sourceCollection.GetEnumerator();

        await using IAsyncEnumerator<int> resultEnumerator =
            result.GetAsyncEnumerator();

        while (enumerator.MoveNext())
        {
            Assert.True(await resultEnumerator.MoveNextAsync());
            Assert.Equal(enumerator.Current, resultEnumerator.Current);
        }

        Assert.False(await resultEnumerator.MoveNextAsync());
    }
#endregion
}
