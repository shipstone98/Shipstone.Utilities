using System;
using System.Collections.Generic;
using Xunit;

using Shipstone.Utilities.Collections;

namespace Shipstone.UtilitiesTest.Collections;

public sealed class ReadOnlyPaginatedCollectionTest
{
    [Fact]
    public void TestEmpty()
    {
        // Act
        ReadOnlyPaginatedCollection<Object> collection =
            ReadOnlyPaginatedCollection<Object>.Empty;

        // Assert
        collection.AssertEmpty();
    }

#region Constructor
#region Invalid arguments
#region Not null
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    [Theory]
    public void TestConstructor_Invalid_NotNull_PageCountLessThanOrEqualToZero(int pageCount)
    {
        // Arrange
        IList<Object> list = Array.Empty<Object>();

        // Act
        ArgumentOutOfRangeException ex =
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ReadOnlyPaginatedCollection<Object>(
                    list,
                    0,
                    0,
                    pageCount
                ));

        // Assert
        Assert.Equal(pageCount, ex.ActualValue);
        Assert.Equal("pageCount", ex.ParamName);
    }

    [InlineData(1, 1, 1, 1)]
    [InlineData(1, 2, 2, 2)]
    [InlineData(1, 3, 3, 2)]
    [Theory]
    public void TestConstructor_Invalid_NotNull_PageCountLessThanOrEqualToPageIndex(
        int count,
        int totalCount,
        int pageIndex,
        int pageCount
    )
    {
        // Arrange
        IList<Object> list = new Object[count];

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentException>(() =>
                new ReadOnlyPaginatedCollection<Object>(
                    list,
                    totalCount,
                    pageIndex,
                    pageCount
                ));

        // Assert
        Assert.Equal("pageCount", ex.ParamName);
    }

    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [Theory]
    public void TestConstructor_Invalid_NotNull_PageIndexLessThanZero(int pageIndex)
    {
        // Arrange
        IList<Object> list = Array.Empty<Object>();

        // Act
        ArgumentOutOfRangeException ex =
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ReadOnlyPaginatedCollection<Object>(
                    list,
                    0,
                    pageIndex,
                    1
                ));

        // Assert
        Assert.Equal(pageIndex, ex.ActualValue);
        Assert.Equal("pageIndex", ex.ParamName);
    }

    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [Theory]
    public void TestConstructor_Invalid_NotNull_TotalCountInvalid_TotalCountLessThanZero(int totalCount)
    {
        // Arrange
        IList<Object> list = Array.Empty<Object>();

        // Act
        ArgumentOutOfRangeException ex =
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new ReadOnlyPaginatedCollection<Object>(
                    list,
                    totalCount,
                    0,
                    1
                ));

        // Assert
        Assert.Equal(totalCount, ex.ActualValue);
        Assert.Equal("totalCount", ex.ParamName);
    }

    [InlineData(1, 2, 0, 1)]
    [InlineData(10, 90, 0, 10)]
    [InlineData(10, 101, 0, 10)]
    [InlineData(10, 99, 9, 10)]
    [Theory]
    public void TestConstructor_Invalid_NotNull_TotalCountInvalid_TotalCountGreaterThanOrEqualToZero(
        int count,
        int totalCount,
        int pageIndex,
        int pageCount
    )
    {
        // Arrange
        IList<Object> list = new Object[count];

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentException>(() =>
                new ReadOnlyPaginatedCollection<Object>(
                    list,
                    totalCount,
                    pageIndex,
                    pageCount
                ));

        // Assert
        Assert.Equal("totalCount", ex.ParamName);
    }
#endregion

    [Fact]
    public void TestConstructor_Invalid_Null()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                new ReadOnlyPaginatedCollection<Object>(null!, 0, 0, 1));

        // Assert
        Assert.Equal("list", ex.ParamName);
    }
#endregion

    [Fact]
    public void TestConstructor_Valid_Empty()
    {
        // Arrange
        IList<Object> list = Array.Empty<Object>();

        // Act
        ReadOnlyPaginatedCollection<Object> collection = new(list, 0, 0, 1);

        // Assert
        collection.AssertEmpty();
    }

    [InlineData(1, 1, 0, 1)]
    [InlineData(1, 2, 0, 2)]
    [InlineData(1, 2, 1, 2)]
    [InlineData(10, 91, 0, 10)]
    [InlineData(10, 100, 0, 10)]
    [InlineData(10, 100, 9, 10)]
    [InlineData(10, Int32.MaxValue, 9, 10)]
    [Theory]
    public void TestConstructor_Valid_NotEmpty(
        int count,
        int totalCount,
        int pageIndex,
        int pageCount
    )
    {
        // Arrange
        int[] list = Internals.CreateInt32Array(count);

        // Act
        ReadOnlyPaginatedCollection<int> collection =
            new(list, totalCount, pageIndex, pageCount);

        // Assert
        collection.AssertEqual(list, totalCount, pageIndex, pageCount);
    }
#endregion
}
