using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shipstone.Utilities.Threading.Tasks;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for querying objects that extend <see cref="Task" />.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Creates an <see cref="IAsyncEnumerable{T}" /> from a <see cref="Task{TResult}" />.
    /// </summary>
    /// <typeparam name="TSource">The type of elements of the source array.</typeparam>
    /// <param name="source">The <see cref="Task{TResult}" /> where the value of <see cref="Task{TResult}.Result" /> contains an array to create an <see cref="IAsyncEnumerable{T}" /> from.</param>
    /// <returns>The created <see cref="IAsyncEnumerable{T}" />.</returns>
    /// <exception cref="ArgumentNullException"><c><paramref name="source" /></c> is <c>null</c>.</exception>
    public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this Task<TSource[]> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return TaskExtensions.AsAsyncEnumerableCore(source);
    }

    private static async IAsyncEnumerable<TSource> AsAsyncEnumerableCore<TSource>(Task<TSource[]> source)
    {
        foreach (TSource item in await source.ConfigureAwait(false))
        {
            yield return item;
        }
    }
}
