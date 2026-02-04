using System;
using System.Security.Cryptography;
using Xunit;

using Shipstone.Utilities.Security.Cryptography;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Security.Cryptography;

public sealed class RandomNumberGeneratorExtensionsTest
{
#region GetNonZeroString method
    [InlineData(Int32.MinValue)]
    [InlineData(-1)]
    [Theory]
    public void TestGetNonZeroString_Invalid_LengthLessThanZero(int length)
    {
        // Arrange
        using MockRandomNumberGenerator rng = new();
        rng._disposeAction = _ => { };

        // Act
        ArgumentOutOfRangeException ex =
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                RandomNumberGeneratorExtensions.GetNonZeroString(rng, length));

        // Assert
        Assert.Equal(length, ex.ActualValue);
        Assert.Equal("length", ex.ParamName);
    }

    [Fact]
    public void TestGetNonZeroString_Invalid_RngNull()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                RandomNumberGeneratorExtensions.GetNonZeroString(null!, 0));

        // Assert
        Assert.Equal("rng", ex.ParamName);
    }

    [InlineData("", 0)]
    [InlineData("12345678912345678", 17)]
    [Theory]
    public void TestGetNonZeroString_Valid(String s, int length)
    {
        // Arrange
        using MockRandomNumberGenerator rng = new();
        rng._disposeAction = _ => { };

        rng._getNonZeroBytesSpanAction = d =>
        {
            for (int i = 0; i < d.Length; i ++)
            {
                d[i] = (byte) i;
            }
        };

        // Act
        String result =
            RandomNumberGeneratorExtensions.GetNonZeroString(rng, length);

        // Assert
        Assert.Equal(s, result);
    }
#endregion
}
