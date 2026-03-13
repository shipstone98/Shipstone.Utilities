using System;
using System.Collections.Generic;
using Xunit;

using Shipstone.Utilities.Collections;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Collections;

public sealed class ListExtensionsTest
{
#region Shuffle method
#region Invalid arguments
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [Theory]
    public void TestShuffle_Invalid_CountLessThanZero(int count)
    {
        // Arrange
        IList<Object> list = Array.Empty<Object>();

        // Act
        ArgumentOutOfRangeException ex =
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                ListExtensions.Shuffle<Object>(list, 0, count));

        // Assert
        Assert.Equal(count, ex.ActualValue);
        Assert.Equal("count", ex.ParamName);
    }

    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [Theory]
    public void TestShuffle_Invalid_IndexLessThanZero(int index)
    {
        // Arrange
        IList<Object> list = Array.Empty<Object>();

        // Act
        ArgumentOutOfRangeException ex =
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                ListExtensions.Shuffle<Object>(list, index, 0));

        // Assert
        Assert.Equal(index, ex.ActualValue);
        Assert.Equal("index", ex.ParamName);
    }

    [Fact]
    public void TestShuffle_Invalid_ListNull()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                ListExtensions.Shuffle<Object>(null!, 0, 0));

        // Assert
        Assert.Equal("list", ex.ParamName);
    }

    [InlineData(0, 0, 1)]
    [InlineData(0, 1, 0)]
    [InlineData(0, 1, 1)]
    [InlineData(10, 0, 11)]
    [InlineData(10, 11, 0)]
    [InlineData(10, 10, 1)]
    [Theory]
    public void TestShuffle_Invalid_RangeInvalid(
        int listCount,
        int index,
        int count
    )
    {
        // Arrange
        IList<Object> list = new Object[listCount];

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentException>(() =>
                ListExtensions.Shuffle(list, index, count));

        // Assert
        Assert.Equal("count", ex.ParamName);
    }
#endregion

#region Valid arguments
    [Fact]
    public void TestShuffle_Valid_Empty()
    {
        // Arrange
        IList<Object> list = Array.Empty<Object>();

        // Act
        ListExtensions.Shuffle(list, 0, 0);

        // Assert
        Assert.Empty(list);
    }

    [Fact]
    public void TestShuffle_Valid_NotEmpty_RandomNotNull()
    {
        // Arrange
        const int COUNT = 10;
        IList<int> list1 = Internals.CreateInt32Array(COUNT);
        IList<int> list2 = Internals.CreateInt32Array(COUNT);
        const int INDEX = 1;
        MockRandom random = new();
        random._nextFunc = (mv, _) => mv;

        // Act
        ListExtensions.Shuffle(list1, INDEX, COUNT - 2, random);
        ListExtensions.Shuffle(list2, INDEX, COUNT - 2, random);

        // Assert
        Assert.Equal(1, list1[0]);
        Assert.Equal(1, list2[0]);
        Assert.Equal(COUNT, list1[^1]);
        Assert.Equal(COUNT, list2[^1]);
        ISet<int> remaining = Internals.CreateInt32Set(COUNT - 2, 2);

        for (int i = 1; i < COUNT - 1; i ++)
        {
            Assert.True(remaining.Remove(i + 1));
            Assert.True(list1[i] == list2[i]);
        }

        Assert.Empty(remaining);
    }

    [Fact]
    public void TestShuffle_Valid_NotEmpty_RandomNull()
    {
        // Arrange
        const int COUNT = 10;
        IList<int> list = Internals.CreateInt32Array(COUNT);

        // Act
        ListExtensions.Shuffle(list, 1, COUNT - 2);

        // Assert
        Assert.Equal(1, list[0]);
        Assert.Equal(COUNT, list[^1]);
        ISet<int> remaining = Internals.CreateInt32Set(COUNT - 2, 2);

        for (int i = 1; i < COUNT - 1; i ++)
        {
            Assert.True(remaining.Remove(i + 1));
        }

        Assert.Empty(remaining);
    }
#endregion
#endregion
}
