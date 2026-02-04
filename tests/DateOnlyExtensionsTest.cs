using System;
using Xunit;

using Shipstone.Utilities;

namespace Shipstone.UtilitiesTest;

public sealed class DateOnlyExtensionsTest
{
#region GetAgeYears method
    [Fact]
    public void TestGetAgeYears_Invalid()
    {
        // Arrange
        DateOnly today = DateOnly.FromDateTime(DateTime.UnixEpoch);
        DateOnly date = today.AddDays(1);

        // Act
        ArgumentOutOfRangeException ex =
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                DateOnlyExtensions.GetAgeYears(date, today));

        // Assert
        Assert.Equal(today, ex.ActualValue);
        Assert.Equal("today", ex.ParamName);
    }

#region Valid arguments
    [Fact]
    public void TestGetAgeYears_Valid_MonthGreaterThan()
    {
        // Arrange
        const int AGE = 18;
        DateOnly today = DateOnly.FromDateTime(DateTime.UnixEpoch);
    
        DateOnly date =
            today
                .AddYears(-AGE)
                .AddMonths(-1);

        // Act
        int age = DateOnlyExtensions.GetAgeYears(date, today);

        // Assert
        Assert.Equal(AGE, age);
    }

    [Fact]
    public void TestGetAgeYears_Valid_MonthEqualTo_DayLessThan()
    {
        // Arrange
        const int AGE = 18;
        DateOnly today = DateOnly.FromDateTime(DateTime.UnixEpoch);
    
        DateOnly date =
            today
                .AddYears(-(AGE + 1))
                .AddDays(1);

        // Act
        int age = DateOnlyExtensions.GetAgeYears(date, today);

        // Assert
        Assert.Equal(AGE, age);
    }

    [InlineData(-1)]
    [InlineData(0)]
    [Theory]
    public void TestGetAgeYears_Valid_MonthEqualTo_DayNotLessThan(int days)
    {
        // Arrange
        const int AGE = 18;
        DateOnly today = DateOnly.FromDateTime(DateTime.UnixEpoch);
    
        DateOnly date =
            today
                .AddYears(-AGE)
                .AddDays(days);

        // Act
        int age = DateOnlyExtensions.GetAgeYears(date, today);

        // Assert
        Assert.Equal(AGE, age);
    }

    [Fact]
    public void TestGetAgeYears_Valid_MonthLessThan()
    {
        // Arrange
        const int AGE = 18;
        DateOnly today = DateOnly.FromDateTime(DateTime.UnixEpoch);
    
        DateOnly date =
            today
                .AddYears(-(AGE + 1))
                .AddMonths(1);

        // Act
        int age = DateOnlyExtensions.GetAgeYears(date, today);

        // Assert
        Assert.Equal(AGE, age);
    }
#endregion
#endregion
}
