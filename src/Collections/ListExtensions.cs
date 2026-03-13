using System;
using System.Collections.Generic;

namespace Shipstone.Utilities.Collections;

/// <summary>
/// Provides a set of <c>static</c> methods (<c>Shared</c> in Visual Basic) methods for manipulating objects that implement <see cref="IList{T}" />.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// Shuffles a range of elements in the specified <see cref="IList{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The <see cref="IList{T}" /> to shuffle.</param>
    /// <param name="index">The zero-based starting index of the range of elements to shuffle.</param>
    /// <param name="count">The number of elements to shuffle.</param>
    /// <param name="random">The <see cref="Random" /> instance to use when shuffling elements, or <c>null</c>.</param>
    /// <exception cref="ArgumentException"><c><paramref name="index" /></c> and <c><paramref name="count" /></c> do not denote a valid range of elements in <c><paramref name="list" /></c>.</exception>
    /// <exception cref="ArgumentNullException"><c><paramref name="list" /></c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><c><paramref name="index" /></c> is less than 0 (zero) -or- <c><paramref name="count" /></c> is less than 0 (zero).</exception>
    public static void Shuffle<T>(
        this IList<T> list,
        int index,
        int count,
        Random? random = null
    )
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0);
        int listCount = list.Count;

        if (index + count > listCount)
        {
            throw new ArgumentException(
                $"{nameof (index)} and {nameof (count)} do not denote a valid range of elements in {nameof (list)}.",
                nameof (count)
            );
        }

        random ??= Random.Shared;

        for (int i = 0; i < count; i ++)
        {
            int j = random.Next(i, count);
            int indexI = index + i;
            int indexJ = index + j;
            (list[indexI], list[indexJ]) = (list[indexJ], list[indexI]);
        }
    }
}
