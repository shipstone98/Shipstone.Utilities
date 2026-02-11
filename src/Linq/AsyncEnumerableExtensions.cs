using System;
using System.Collections.Generic;
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
}
