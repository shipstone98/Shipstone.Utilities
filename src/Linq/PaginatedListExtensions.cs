using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Shipstone.Utilities.Collections;

namespace Shipstone.Utilities.Linq;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for querying objects that implement <see cref="IReadOnlyPaginatedList{T}" />.
/// </summary>
public static class PaginatedListExtensions
{
    /// <summary>
    /// Asynchronously projects each element of a paginated list into a new form by incorporating the element's index.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of the source paginated list.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by the transform function.</typeparam>
    /// <param name="source">A paginated list of values to invoke a transform function on.</param>
    /// <param name="selector">An asynchronous transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <returns>An <see cref="IReadOnlyPaginatedList{T}" /> whose elements are the result of invoking <c><paramref name="selector" /></c> on each element of <c><paramref name="source" /></c>.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="source" /></c> is <c>null</c> -or- <c><paramref name="selector" /></c> is <c>null</c>.</exception>
    public static IReadOnlyPaginatedList<TResult> Select<TSource, TResult>(
        this IReadOnlyPaginatedList<TSource> source,
        Func<TSource, int, TResult> selector
    )
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        IList<TResult> list =
            (source as IEnumerable<TSource>)
                .Select(selector)
                .ToArray();

        return new ReadOnlyPaginatedCollection<TResult>(
            list,
            source.TotalCount,
            source.PageIndex,
            source.PageCount
        );
    }
}
