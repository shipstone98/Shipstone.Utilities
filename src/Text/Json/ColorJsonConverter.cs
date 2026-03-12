using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shipstone.Utilities.Text.Json;

/// <summary>
/// Converts a color to or from JSON.
/// </summary>
public class ColorJsonConverter : JsonConverter<Color>
{
    /// <inheritdoc />
    public override Color Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        byte a = Byte.MaxValue;
        byte r = 0;
        byte g = 0;
        byte b = 0;

        try
        {
            if (!(reader.Read() && reader.TokenType == JsonTokenType.StartObject))
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                String? propertyName = reader.GetString();

                if (!reader.Read())
                {
                    throw new JsonException();
                }

                switch (propertyName?.ToLowerInvariant())
                {
                    case "a":
                        a = reader.GetByte();
                        break;
                    case "b":
                        b = reader.GetByte();
                        break;
                    case "g":
                        g = reader.GetByte();
                        break;
                    case "r":
                        r = reader.GetByte();
                        break;
                }
            }
        }

        catch (JsonException)
        {
            return Color.FromArgb(Byte.MaxValue, 0, 0, 0);
        }

        return Color.FromArgb(a, r, g, b);
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        Color value,
        JsonSerializerOptions options
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStartObject();
        writer.WriteNumber("a", value.A);
        writer.WriteNumber("r", value.R);
        writer.WriteNumber("g", value.G);
        writer.WriteNumber("b", value.B);
        writer.WriteEndObject();
    }
}
