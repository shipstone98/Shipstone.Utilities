using System;
using System.Security.Cryptography;
using Xunit;

using Shipstone.Utilities.Security.Cryptography;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Security.Cryptography;

public sealed class ConcurrentRandomNumberGeneratorTest
{
    [Fact]
    public void TestConstructor_Invalid()
    {
        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                new ConcurrentRandomNumberGenerator(null!));

        // Assert
        Assert.Equal("rng", ex.ParamName);
    }

    [Fact]
    public void TestConstructor_Valid()
    {
        // Arrange
        using MockRandomNumberGenerator rng = new();
        rng._disposeAction = _ => { };

        // Act
        using ConcurrentRandomNumberGenerator crng = new(rng);

        // Nothing to assert
    }

    [Fact]
    public void TestDispose()
    {
        // Arrange
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng = new(rng);
        rng._disposeAction = _ => { };

        // Act
        crng.Dispose();

        // Nothing to assert
    }

#region Equals method
    [InlineData(false)]
    [InlineData(true)]
    [Theory]
    public void TestEquals_ConcurrentRandomNumberGenerator(bool areEqual)
    {
        // Arrange
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng1 = new(rng);
        using ConcurrentRandomNumberGenerator crng2 = new(rng);
        rng._disposeAction = _ => { };
        rng._equalsFunc = _ => areEqual;

        // Act
        bool result1 = crng1.Equals(crng2);
        bool result2 = crng2.Equals(crng1);

        // Assert
        Assert.Equal(areEqual, result1);
        Assert.Equal(areEqual, result2);
    }

    [Fact]
    public void TestEquals_Object()
    {
        // Arrange
        using MockRandomNumberGenerator rng = new();
        Object obj = new();
        using ConcurrentRandomNumberGenerator crng1 = new(rng);
        using ConcurrentRandomNumberGenerator crng2 = new(rng);
        rng._disposeAction = _ => { };

        // Act
        bool result1 = crng1.Equals(obj);
        bool result2 = crng2.Equals(obj);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [InlineData(false)]
    [InlineData(true)]
    [Theory]
    public void TestEquals_RandomNumberGenerator(bool areEqual)
    {
        // Arrange
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng1 = new(rng);
        using ConcurrentRandomNumberGenerator crng2 = new(rng);
        rng._disposeAction = _ => { };
        rng._equalsFunc = _ => areEqual;

        // Act
        bool result1 = crng1.Equals(rng);
        bool result2 = crng2.Equals(rng);

        // Assert
        Assert.Equal(areEqual, result1);
        Assert.Equal(areEqual, result2);
    }
#endregion

    [Fact]
    public void TestGetHashCode()
    {
        // Arrange
        const int HASH_CODE = 31;
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng1 = new(rng);
        using ConcurrentRandomNumberGenerator crng2 = new(rng);
        rng._disposeAction = _ => { };
        rng._getHashCodeFunc = () => HASH_CODE;

        // Act
        int hashCode1 = crng1.GetHashCode();
        int hashCode2 = crng2.GetHashCode();

        // Assert
        Assert.Equal(HASH_CODE, hashCode1);
        Assert.Equal(HASH_CODE, hashCode2);
    }

#region GetBytes methods
    [Fact]
    public void TestGetBytes_Array()
    {
        // Arrange
        const int LENGTH = 17;
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng = new(rng);
        byte[] data = new byte[LENGTH];
        rng._disposeAction = _ => { };

        rng._getBytesArrayAction = d =>
        {
            for (int i = 0; i < d.Length; i ++)
            {
                d[i] = (byte) i;
            }
        };

        // Act
        crng.GetBytes(data);

        // Assert
        Assert.Equal(LENGTH, data.Length);

        for (int i = 0; i < LENGTH; i ++)
        {
            Assert.Equal(i, data[i]);
        }
    }

    [Fact]
    public void TestGetBytes_Array_Int32_Int32()
    {
        // Arrange
        const int LENGTH = 17;
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng = new(rng);
        byte[] data = new byte[LENGTH];
        rng._disposeAction = _ => { };

        rng._getBytesArrayInt32Int32Action = (d, o, c) =>
        {
            for (int i = 0; i < c; i ++)
            {
                d[i + o] = (byte) (i + o);
            }
        };

        // Act
        crng.GetBytes(data, 1, LENGTH - 2);

        // Assert
        Assert.Equal(LENGTH, data.Length);
        Assert.Equal(0, data[0]);
        Assert.Equal(0, data[^1]);

        for (int i = 1; i < LENGTH - 2; i ++)
        {
            Assert.Equal(i, data[i]);
        }
    }

    [Fact]
    public void TestGetBytes_Span()
    {
        // Arrange
        const int LENGTH = 17;
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng = new(rng);
        Span<byte> data = stackalloc byte[LENGTH];
        rng._disposeAction = _ => { };

        rng._getBytesSpanAction = d =>
        {
            for (int i = 0; i < d.Length; i ++)
            {
                d[i] = (byte) i;
            }
        };

        // Act
        crng.GetBytes(data);

        // Assert
        Assert.Equal(LENGTH, data.Length);

        for (int i = 0; i < LENGTH; i ++)
        {
            Assert.Equal(i, data[i]);
        }
    }
#endregion

    [Fact]
    public void TestGetNonZeroBytes_Array()
    {
        // Arrange
        const int LENGTH = 17;
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng = new(rng);
        byte[] data = new byte[LENGTH];
        rng._disposeAction = _ => { };

        rng._getNonZeroBytesArrayAction = d =>
        {
            for (int i = 0; i < d.Length; i ++)
            {
                d[i] = (byte) i;
            }
        };

        // Act
        crng.GetNonZeroBytes(data);

        // Assert
        Assert.Equal(LENGTH, data.Length);

        for (int i = 0; i < LENGTH; i ++)
        {
            Assert.Equal(i, data[i]);
        }
    }

    [Fact]
    public void TestGetNonZeroBytes_Span()
    {
        // Arrange
        const int LENGTH = 17;
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng = new(rng);
        Span<byte> data = stackalloc byte[LENGTH];
        rng._disposeAction = _ => { };

        rng._getNonZeroBytesSpanAction = d =>
        {
            for (int i = 0; i < d.Length; i ++)
            {
                d[i] = (byte) i;
            }
        };

        // Act
        crng.GetNonZeroBytes(data);

        // Assert
        Assert.Equal(LENGTH, data.Length);

        for (int i = 0; i < LENGTH; i ++)
        {
            Assert.Equal(i, data[i]);
        }
    }

    [Fact]
    public void TestToString()
    {
        // Arrange
        const String STRING = "My string";
        using MockRandomNumberGenerator rng = new();
        using ConcurrentRandomNumberGenerator crng = new(rng);
        rng._disposeAction = _ => { };
        rng._toStringFunc = () => STRING;

        // Act
        String? s = crng.ToString();

        // Assert
        Assert.Equal(STRING, s);
    }
}
