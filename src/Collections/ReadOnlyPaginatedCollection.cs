using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shipstone.Utilities.Collections;

/// <summary>
/// Provides the base class for a generic read-only, paginated collection.
/// </summary>
/// <typeparam name="T">The type of elements in the paginated collection.</typeparam>
public class ReadOnlyPaginatedCollection<T>
    : ReadOnlyCollection<T>, IReadOnlyPaginatedList<T>
{
    private static readonly ReadOnlyPaginatedCollection<T> _empty;

    private readonly int _pageCount;
    private readonly int _pageIndex;
    private readonly int _totalCount;

    /// <summary>
    /// Gets an empty <see cref="ReadOnlyPaginatedCollection{T}" />.
    /// </summary>
    /// <value>An empty <see cref="ReadOnlyPaginatedCollection{T}" />.</value>
    public static new ReadOnlyPaginatedCollection<T> Empty =>
        ReadOnlyPaginatedCollection<T>._empty;

    /// <summary>
    /// Gets the number of pages in the collection.
    /// </summary>
    /// <value>The number of pages in the collection.</value>
    public int PageCount => this._pageCount;

    /// <summary>
    /// Gets the zero-based index of the page in the collection.
    /// </summary>
    /// <value>The zero-based index of the page in the collection.</value>
    public int PageIndex => this._pageIndex;

    /// <summary>
    /// Gets the total number of elements in all pages in the collection.
    /// </summary>
    /// <value>The total number of elements in all pages in the collection.</value>
    public int TotalCount => this._totalCount;

    static ReadOnlyPaginatedCollection()
    {
        IList<T> list = Array.Empty<T>();
        ReadOnlyPaginatedCollection<T>._empty = new(list, 0, 0, 1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyPaginatedCollection{T}" /> class that is a read-only, paginated wrapper around the specified list.
    /// </summary>
    /// <param name="list">The list to wrap.</param>
    /// <param name="totalCount">The total number of elements in all pages for the new collection.</param>
    /// <param name="pageIndex">The zero-based index of the page for the new collection.</param>
    /// <param name="pageCount">The number of pages for the new collection.</param>
    /// <exception cref="ArgumentException"><c><paramref name="pageCount" /></c> is less than or equal to <c><paramref name="pageIndex" /></c>.</exception>
    /// <exception cref="ArgumentNullException"><c><paramref name="list" /></c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><c><paramref name="totalCount" /></c> is less than 0 (zero) -or- <c><paramref name="pageIndex" /></c> is less than 0 (zero) -or- <c><paramref name="pageCount" /></c> is less than or equal to 0 (zero)</exception>
    public ReadOnlyPaginatedCollection(
        IList<T> list,
        int totalCount,
        int pageIndex,
        int pageCount
    )
        : base(list)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(totalCount, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageIndex, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageCount, 0);
        int listCount = list.Count;
        int totalCountMin;
        int totalCountMax;

        if (pageCount <= pageIndex)
        {
            throw new ArgumentException(
                $"{nameof (pageCount)} is less than or equal to {nameof (pageIndex)}.",
                nameof (pageCount)
            );
        }

        if (pageCount > 1)
        {
            if (pageIndex == pageCount - 1)
            {
                totalCountMin = pageCount * listCount;
                totalCountMax = Int32.MaxValue;
            }

            else
            {
                totalCountMin = (pageCount - 1) * listCount + 1;
                totalCountMax = pageCount * listCount;
            }
        }

        else
        {
            totalCountMin = totalCountMax = listCount;
        }

        if (totalCount < totalCountMin || totalCount > totalCountMax)
        {
            throw new ArgumentException(
                $"{nameof (totalCount)} is not valid for the specified properties.",
                nameof (totalCount)
            );
        }

        this._pageCount = pageCount;
        this._pageIndex = pageIndex;
        this._totalCount = totalCount;
    }
}
