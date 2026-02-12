using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

using Shipstone.Utilities.Collections;
using Shipstone.Utilities.Text.Json;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Text.Json;

public sealed class PaginatedListJsonConverterTest
{
    [Fact]
    public void TestRead()
    {
        // Arrange
        PaginatedListJsonConverter<Object> converter = new();
        Utf8JsonReader reader = new();

        // Act and assert
        try
        {
            converter.Read(
                ref reader,
                typeof (Object),
                new JsonSerializerOptions { }
            );
        }

        catch (NotSupportedException)
        {
            // Do nothing - method should throw
        }

        catch (Exception)
        {
            Assert.Fail();
        }
    }

#region Write method
#region Invalid arguments
    [Fact]
    public void TestWrite_Invalid_OptionsNull()
    {
        // Arrange
        PaginatedListJsonConverter<Object> converter = new();
        using MockStream stream = new();
        stream._canWriteFunc = () => true;
        Utf8JsonWriter writer = new(stream);

        IReadOnlyPaginatedList<Object> val =
            new MockReadOnlyPaginatedList<Object>();

        stream._closeAction = () => { };

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                converter.Write(writer, val, null!));

        // Assert
        Assert.Equal("options", ex.ParamName);
    }

    [Fact]
    public void TestWrite_Invalid_ValueNull()
    {
        // Arrange
        PaginatedListJsonConverter<Object> converter = new();
        using MockStream stream = new();
        stream._canWriteFunc = () => true;
        Utf8JsonWriter writer = new(stream);
        stream._closeAction = () => { };

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                converter.Write(writer, null!, new JsonSerializerOptions { }));

        // Assert
        Assert.Equal("value", ex.ParamName);
    }

    [Fact]
    public void TestWrite_Invalid_WriterNull()
    {
        // Arrange
        PaginatedListJsonConverter<Object> converter = new();

        IReadOnlyPaginatedList<Object> val =
            new MockReadOnlyPaginatedList<Object>();

        // Act
        ArgumentException ex =
            Assert.Throws<ArgumentNullException>(() =>
                converter.Write(null!, val, new JsonSerializerOptions { }));

        // Assert
        Assert.Equal("writer", ex.ParamName);
    }
#endregion

    [Fact]
    public void TestWriter_Valid_Empty()
    {
#region Arrange
        // Arrange
        IEnumerable<Object> collection = Array.Empty<Object>();
        ICollection<byte> bytes = new List<byte>();
        PaginatedListJsonConverter<Object> converter = new();
        using MockStream stream = new();
        stream._canWriteFunc = () => true;
        Utf8JsonWriter writer = new(stream);
        MockReadOnlyPaginatedList<Object> val = new();
        val._pageCountFunc = () => 1;
        val._pageIndexFunc = () => 0;
        val._getEnumeratorFunc = collection.GetEnumerator;
        val._totalCountFunc = () => 0;

        stream._writeAction = b =>
        {
            foreach (byte by in b)
            {
                bytes.Add(by);
            }
        };

        stream._flushAction = () => { };
        stream._closeAction = () => { };
#endregion

        // Act
        converter.Write(writer, val, new JsonSerializerOptions { });

        // Assert
        writer.Flush();
        byte[] byteArray = new byte[bytes.Count];
        bytes.CopyTo(byteArray, 0);
        String s = Encoding.UTF8.GetString(byteArray);

        Assert.Equal(
            $"{{\"count\":1,\"index\":0,\"items\":[],\"totalCount\":0}}",
            s
        );
    }

    [Fact]
    public void TestWriter_Valid_NotEmpty()
    {
#region Arrange
        // Arrange
        const int COUNT = 5;
        IEnumerable<int> collection = new int[COUNT] { 11, 12, 13, 14, 15 };
        const int TOTAL_COUNT = 12345;
        const int PAGE_INDEX = 17;
        const int PAGE_COUNT = 23;
        ICollection<byte> bytes = new List<byte>();
        PaginatedListJsonConverter<int> converter = new();
        using MockStream stream = new();
        stream._canWriteFunc = () => true;
        Utf8JsonWriter writer = new(stream);
        MockReadOnlyPaginatedList<int> val = new();
        val._pageCountFunc = () => PAGE_COUNT;
        val._pageIndexFunc = () => PAGE_INDEX;
        val._getEnumeratorFunc = collection.GetEnumerator;
        val._totalCountFunc = () => TOTAL_COUNT;

        stream._writeAction = b =>
        {
            foreach (byte by in b)
            {
                bytes.Add(by);
            }
        };

        stream._flushAction = () => { };
        stream._closeAction = () => { };
#endregion

        // Act
        converter.Write(writer, val, new JsonSerializerOptions { });

        // Assert
        writer.Flush();
        byte[] byteArray = new byte[bytes.Count];
        bytes.CopyTo(byteArray, 0);
        String s = Encoding.UTF8.GetString(byteArray);

        Assert.Equal(
            $"{{\"count\":{PAGE_COUNT},\"index\":{PAGE_INDEX},\"items\":[11,12,13,14,15],\"totalCount\":{TOTAL_COUNT}}}",
            s
        );
    }
#endregion
}
