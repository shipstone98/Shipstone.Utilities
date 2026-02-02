using System.Collections.Generic;

namespace Shipstone.Utilities.Collections;

/// <summary>
/// Represents a read-only, paginated collection of elements that can be accessed by index.
/// </summary>
/// <typeparam name="T">The type of elements in the pagintaed list.</typeparam>
public interface IReadOnlyPaginatedList<out T> : IReadOnlyList<T>
{
    /// <summary>
    /// Gets the number of pages in the list.
    /// </summary>
    /// <value>The number of pages in the list.</value>
    int PageCount { get; }

    /// <summary>
    /// Gets the zero-based index of the page in the list.
    /// </summary>
    /// <value>The zero-based index of the page in the list.</value>
    int PageIndex { get; }

    /// <summary>
    /// Gets the total number of elements in all pages in the list.
    /// </summary>
    /// <value>The total number of elements in all pages in the list.</value>
    int TotalCount { get; }
}
