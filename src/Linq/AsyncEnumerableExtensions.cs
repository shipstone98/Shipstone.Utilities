using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Shipstone.Utilities.Linq;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for querying objects that implement <see cref="IAsyncEnumerable{T}" />.
/// </summary>
public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// Asynchronously creates a list from an <see cref="IAsyncEnumerable{T}" />.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of the source collection.</typeparam>
    /// <param name="source">The <see cref="IAsyncEnumerable{T}" /> to create a list from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous create operation. The value of <see cref="Task{TResult}.Result" /> contains the created <see cref="List{T}" />.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="source" /></c> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled.</exception>
    public static Task<List<TSource>> ToListAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(source);

        return AsyncEnumerableExtensions.ToListAsyncCore(
            source,
            cancellationToken
        );
    }

    private static async Task<List<TSource>> ToListAsyncCore<TSource>(
        IAsyncEnumerable<TSource> source,
        CancellationToken cancellationToken
    )
    {
        List<TSource> list = new();

        await foreach (TSource item in source
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false))
        {
            list.Add(item);
        }

        return list;
    }

    /// <summary>
    /// Asynchronously creates a sorted set from an <see cref="IAsyncEnumerable{T}" /> using the specified comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of the source collection.</typeparam>
    /// <param name="source">The <see cref="IAsyncEnumerable{T}" /> to create a sorted set from.</param>
    /// <param name="comparer">An <see cref="IComparer{T}" /> to compare elements, or <c>null</c> to use the <see cref="Comparer{T}.Default" />.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous create operation. The value of <see cref="Task{TResult}.Result" /> contains the created <see cref="SortedSet{T}" />.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="source" /></c> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled.</exception>
    public static Task<SortedSet<TSource>> ToSortedSetAsync<TSource>(
        this IAsyncEnumerable<TSource> source,
        IComparer<TSource>? comparer = null,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(source);

        return AsyncEnumerableExtensions.ToSortedSetAsyncCore(
            source,
            comparer,
            cancellationToken
        );
    }

    private static async Task<SortedSet<TSource>> ToSortedSetAsyncCore<TSource>(
        IAsyncEnumerable<TSource> source,
        IComparer<TSource>? comparer,
        CancellationToken cancellationToken
    )
    {
        SortedSet<TSource> sortedSet = new(comparer);

        await foreach (TSource item in source
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false))
        {
            sortedSet.Add(item);
        }

        return sortedSet;
    }

    /// <summary>
    /// Filters a sequence of values based on non-nullability.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of the source collection.</typeparam>
    /// <param name="source">The <see cref="IAsyncEnumerable{T}" /> to filter.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous filter operation. The value of <see cref="Task{TResult}.Result" /> contains an <see cref="IAsyncEnumerable{T}" /> that contains elements from <c><paramref name="source" /></c> that are not <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="source" /></c> is <c>null</c>.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled.</exception>
    public static IAsyncEnumerable<TSource> WhereNotNullAsync<TSource>(
        this IAsyncEnumerable<TSource?> source,
        CancellationToken cancellationToken = default
    )
        where TSource : class
    {
        ArgumentNullException.ThrowIfNull(source);

        return AsyncEnumerableExtensions.WhereNotNullAsyncCore(
            source,
            cancellationToken
        );
    }

    private static async IAsyncEnumerable<TSource> WhereNotNullAsyncCore<TSource>(
        this IAsyncEnumerable<TSource?> source,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        await foreach (TSource? item in source
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false))
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }
}
