using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

using Shipstone.Utilities.Collections;
using Shipstone.Utilities.Text.Json;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Text.Json;

public sealed class PaginatedListJsonConverterFactoryTest
{
    [Fact]
    public void TestCanConvert_Invalid()
    {
        // Arrange
        PaginatedListJsonConverterFactory factory = new();

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                factory.CanConvert(null!));

        // Assert
        Assert.Equal("typeToConvert", ex.ParamName);
    }

    [Fact]
    public void TestCanConvert_Valid()
    {
        // Arrange
        PaginatedListJsonConverterFactory factory = new();

        IReadOnlyDictionary<Type, bool> types = new Dictionary<Type, bool>
        {
            { typeof (IReadOnlyPaginatedList<>), false },
            { typeof (IReadOnlyPaginatedList<Object>), true },
            { typeof (MockReadOnlyPaginatedList<>), false },
            { typeof (MockReadOnlyPaginatedList<Object>), true },
            { typeof (Object), false },
            { typeof (IReadOnlyList<>), false },
            { typeof (IReadOnlyList<Object>), false },
            { typeof (List<>), false },
            { typeof (List<Object>), false }
        };

        foreach (KeyValuePair<Type, bool> type in types)
        {
            // Act
            bool result = factory.CanConvert(type.Key);

            // Assert
            Assert.Equal(type.Value, result);
        }
    }

#region CreateConverter method
    [Fact]
    public void TestCreateConverter_Invalid_OptionsNull()
    {
        // Arrange
        PaginatedListJsonConverterFactory factory = new();

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                factory.CreateConverter(typeof (Object), null!));

        // Assert
        Assert.Equal("options", ex.ParamName);
    }

    [Fact]
    public void TestCreateConverter_Invalid_TypeToConvertNull()
    {
        // Arrange
        PaginatedListJsonConverterFactory factory = new();

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                factory.CreateConverter(null!, new JsonSerializerOptions { }));

        // Assert
        Assert.Equal("typeToConvert", ex.ParamName);
    }

    [Fact]
    public void TestCreateConverter_Valid_Convertible()
    {
        // Arrange
        PaginatedListJsonConverterFactory factory = new();

        IEnumerable<Type> types = new Type[]
        {
            typeof (IReadOnlyPaginatedList<Object>),
            typeof (MockReadOnlyPaginatedList<Object>)
        };

        foreach (Type type in types)
        {
            // Act
            Object? result =
                factory.CreateConverter(type, new JsonSerializerOptions { });

            // Assert
            Assert.NotNull(result);
        }
    }

    [Fact]
    public void TestCreateConverter_Valid_NotConvertible()
    {
        // Arrange
        PaginatedListJsonConverterFactory factory = new();

        IEnumerable<Type> types = new Type[]
        {
            typeof (IReadOnlyPaginatedList<>),
            typeof (MockReadOnlyPaginatedList<>),
            typeof (Object),
            typeof (IReadOnlyList<>),
            typeof (IReadOnlyList<Object>),
            typeof (List<>),
            typeof (List<Object>)
        };

        foreach (Type type in types)
        {
            // Act
            Object? result =
                factory.CreateConverter(type, new JsonSerializerOptions { });

            // Assert
            Assert.Null(result);
        }
    }
#endregion
}
