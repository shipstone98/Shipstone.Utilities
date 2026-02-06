using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Shipstone.Utilities.Linq;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for querying objects that implement <see cref="IEnumerable{T}" />.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Asynchronously projects each element of a sequence into a new form by incorporating the element's index.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of the source collection.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by the transform function.</typeparam>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">An asynchronous transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}" /> whose elements are the result of invoking <c><paramref name="selector" /></c> on each element of <c><paramref name="source" /></c>.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="source" /></c> is <c>null</c> -or- <c><paramref name="selector" /></c> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled.</exception>
    public static IAsyncEnumerable<TResult> SelectAsync<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, int, CancellationToken, Task<TResult>> selector,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        return EnumerableExtensions.SelectAsyncCore(
            source,
            selector,
            cancellationToken
        );
    }

    internal static async IAsyncEnumerable<TResult> SelectAsyncCore<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, int, CancellationToken, Task<TResult>> selector,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        int index = -1;

        foreach (TSource item in source)
        {
            checked
            {
                ++ index;
            }

            yield return await selector(item, index, cancellationToken);
        }
    }

    /// <summary>
    /// Asynchronously projects each element of a sequence to an <see cref="IEnumerable{T}" />, and flattens the resulting sequences into one sequence.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of the source collection.</typeparam>
    /// <typeparam name="TResult">The type of the elements of the sequence returned by the transform function.</typeparam>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">An asynchronous transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}" /> whose elements are the result of invoking <c><paramref name="selector" /></c> on each element of <c><paramref name="source" /></c>.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="source" /></c> is <c>null</c> -or- <c><paramref name="selector" /></c> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled.</exception>
    public static IAsyncEnumerable<TResult> SelectManyAsync<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, int, CancellationToken, IAsyncEnumerable<TResult>> selector,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        return EnumerableExtensions.SelectManyAsyncCore(
            source,
            selector,
            cancellationToken
        );
    }

    private static async IAsyncEnumerable<TResult> SelectManyAsyncCore<TSource, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, int, CancellationToken, IAsyncEnumerable<TResult>> selector,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        int index = -1;

        foreach (TSource item in source)
        {
            checked
            {
                ++ index;
            }

            await foreach (TResult resultItem in selector(
                item,
                index,
                cancellationToken
            ))
            {
                yield return resultItem;
            }
        }
    }
}
