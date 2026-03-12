using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

using Shipstone.Utilities.Text.Json;

using Shipstone.UtilitiesTest.Mocks;

namespace Shipstone.UtilitiesTest.Text.Json;

public sealed class ColorJsonConverterTest
{
    [InlineData("", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{", Byte.MaxValue, 0, 0, 0)]
    [InlineData("}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"r\":}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"g\":}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"b\":}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"\":}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":255}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"r\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"r\":255}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"g\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"g\":255}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":0,\"r\":255}", 0, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":255,\"r\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":255,\"r\":255}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":0,\"g\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":0,\"g\":255}", 0, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":255,\"g\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":255,\"g\":255}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":0,\"b\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":0,\"b\":255}", 0, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":255,\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":255}", 0, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":0}", 0, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":255}", 0, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":255}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":0}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":255}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"a\":0,\"r\":0,\"b\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":0,\"r\":0,\"b\":255}", 0, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":255,\"b\":0}", 0, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":0,\"r\":255,\"b\":255}", 0, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":0,\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":255,\"r\":0,\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":255,\"b\":0}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":255,\"r\":255,\"b\":255}", Byte.MaxValue, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":0,\"b\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":0,\"b\":255}", 0, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":255,\"b\":0}", 0, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":255,\"b\":255}", 0, 0, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":0,\"b\":0}", 0, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":0,\"b\":255}", 0, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":255,\"b\":0}", 0, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":255,\"b\":255}", 0, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":0,\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":0,\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":255,\"b\":0}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":255,\"b\":255}", Byte.MaxValue, 0, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":0,\"b\":0}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":0,\"b\":255}", Byte.MaxValue, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":255,\"b\":0}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":255,\"b\":255}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"r\":0,\"g\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"r\":0,\"g\":255}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"r\":255,\"g\":0}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"r\":255,\"g\":255}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"r\":0,\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"r\":0,\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"r\":255,\"b\":0}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"r\":255,\"b\":255}", Byte.MaxValue, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"r\":0,\"g\":0,\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"r\":0,\"g\":0,\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"r\":0,\"g\":255,\"b\":0}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"r\":0,\"g\":255,\"b\":255}", Byte.MaxValue, 0, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"r\":255,\"g\":0,\"b\":0}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"r\":255,\"g\":0,\"b\":255}", Byte.MaxValue, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"r\":255,\"g\":255,\"b\":0}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"r\":255,\"g\":255,\"b\":255}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"g\":0,\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"g\":0,\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"g\":255,\"b\":0}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"g\":255,\"b\":255}", Byte.MaxValue, 0, Byte.MaxValue, Byte.MaxValue)]
    [Theory]
    public void TestRead(String s, byte alpha, byte red, byte green, byte blue)
    {
        // Arrange
        JsonConverter<Color> converter = new ColorJsonConverter();
        ReadOnlySpan<byte> bytes = Encoding.UTF8.GetBytes(s);
        Utf8JsonReader reader = new(bytes);

        // Act
        Color color =
            converter.Read(
                ref reader,
                typeof (Color),
                new JsonSerializerOptions { }
            );

        // Assert
        Assert.Equal(alpha, color.A);
        Assert.Equal(red, color.R);
        Assert.Equal(green, color.G);
        Assert.Equal(blue, color.B);
    }

    [InlineData("{\"a\":0,\"r\":0,\"g\":0,\"b\":0}", 0, 0, 0, 0)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":0,\"b\":255}", 0, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":255,\"b\":0}", 0, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":0,\"r\":0,\"g\":255,\"b\":255}", 0, 0, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":0,\"b\":0}", 0, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":0,\"b\":255}", 0, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":255,\"b\":0}", 0, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"a\":0,\"r\":255,\"g\":255,\"b\":255}", 0, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":0,\"b\":0}", Byte.MaxValue, 0, 0, 0)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":0,\"b\":255}", Byte.MaxValue, 0, 0, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":255,\"b\":0}", Byte.MaxValue, 0, Byte.MaxValue, 0)]
    [InlineData("{\"a\":255,\"r\":0,\"g\":255,\"b\":255}", Byte.MaxValue, 0, Byte.MaxValue, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":0,\"b\":0}", Byte.MaxValue, Byte.MaxValue, 0, 0)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":0,\"b\":255}", Byte.MaxValue, Byte.MaxValue, 0, Byte.MaxValue)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":255,\"b\":0}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, 0)]
    [InlineData("{\"a\":255,\"r\":255,\"g\":255,\"b\":255}", Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue)]
    [Theory]
    public void TestWrite(
        String s,
        byte alpha,
        byte red,
        byte green,
        byte blue
    )
    {
        // Arrange
        JsonConverter<Color> converter = new ColorJsonConverter();
        ICollection<byte> bytes = new List<byte>();
        MockStream stream = new();
        stream._canWriteFunc = () => true;
        Utf8JsonWriter writer = new(stream);
        Color color = Color.FromArgb(alpha, red, green, blue);

        stream._writeAction = by =>
        {
            foreach (byte b in by)
            {
                bytes.Add(b);
            }
        };

        stream._flushAction = () => { };

        // Act
        converter.Write(writer, color, new JsonSerializerOptions { });

        // Assert
        writer.Flush();
        byte[] array = new byte[bytes.Count];
        bytes.CopyTo(array, 0);
        String stringActual = Encoding.UTF8.GetString(array);
        Assert.Equal(s, stringActual);
    }
}
