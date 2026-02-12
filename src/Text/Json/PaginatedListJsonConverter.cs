using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shipstone.Utilities.Collections;

namespace Shipstone.Utilities.Text.Json;

/// <summary>
/// Converts a paginated list to JSON.
/// </summary>
/// <typeparam name="T">The type of elements in the paginated list.</typeparam>
public class PaginatedListJsonConverter<T>
    : JsonConverter<IReadOnlyPaginatedList<T>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedListJsonConverter{T}" /> class.
    /// </summary>
    public PaginatedListJsonConverter() { }

    /// <inheritdoc />
    public override IReadOnlyPaginatedList<T>? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);
        throw new NotSupportedException("The JSON converter does not support reading.");
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        IReadOnlyPaginatedList<T> value,
        JsonSerializerOptions options
    )
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(options);
        writer.WriteStartObject();
        writer.WriteNumber("count", value.PageCount);
        writer.WriteNumber("index", value.PageIndex);
        writer.WriteStartArray("items");

        foreach (T item in value)
        {
            String json = JsonSerializer.Serialize(item, options);
            writer.WriteRawValue(json);
        }

        writer.WriteEndArray();
        writer.WriteNumber("totalCount", value.TotalCount);
        writer.WriteEndObject();
    }
}
